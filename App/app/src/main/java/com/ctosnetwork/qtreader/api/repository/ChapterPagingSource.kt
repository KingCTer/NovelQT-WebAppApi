/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:30, 27/10/2021
 */

package com.ctosnetwork.qtreader.api.repository

import androidx.paging.PagingSource
import androidx.paging.PagingState
import com.ctosnetwork.qtreader.api.data.DataChapter
import com.ctosnetwork.qtreader.api.service.ChapterService

private const val CHAPTER_STARTING_PAGE_INDEX = 1
private const val NETWORK_CHAPTER_SIZE = 200

class ChapterPagingSource(
    private val service: ChapterService,
    private val bookId: String,
    private val query: String
) : PagingSource<Int, DataChapter>() {

    override suspend fun load(params: LoadParams<Int>): LoadResult<Int, DataChapter> {
        val page = params.key ?: CHAPTER_STARTING_PAGE_INDEX
        return try {
            val response = service.getChapterList(bookId, page, NETWORK_CHAPTER_SIZE, query)
            val data = response.data
            LoadResult.Page(
                data = data,
                prevKey = if (page == CHAPTER_STARTING_PAGE_INDEX) null else page - 1,
                nextKey = if (page == response.totalPages || response.totalPages == 0) null else page + 1
            )
        } catch (exception: Exception) {
            LoadResult.Error(exception)
        }
    }

    override fun getRefreshKey(state: PagingState<Int, DataChapter>): Int? {
        return state.anchorPosition?.let { anchorPosition ->
            // This loads starting from previous page, but since PagingConfig.initialLoadSize spans
            // multiple pages, the initial load will still load items centered around
            // anchorPosition. This also prevents needing to immediately launch prepend due to
            // prefetchDistance.
            state.closestPageToPosition(anchorPosition)?.prevKey
        }
    }
}