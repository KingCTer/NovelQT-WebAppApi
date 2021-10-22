/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:53, 22/10/2021
 */

package com.ctosnetwork.qtreader.api.repository

import androidx.paging.PagingSource
import androidx.paging.PagingState
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.api.service.BookService

private const val BOOK_STARTING_PAGE_INDEX = 1

class BookPagingSource (
    private val service: BookService,
    private val query: String
) : PagingSource<Int, DataBook>() {

    override suspend fun load(params: LoadParams<Int>): LoadResult<Int, DataBook> {
        val page = params.key ?: BOOK_STARTING_PAGE_INDEX
        return try {
            val response = service.getBooks(page, params.loadSize, query)
            val photos = response.data
            LoadResult.Page(
                data = photos,
                prevKey = if (page == BOOK_STARTING_PAGE_INDEX) null else page - 1,
                nextKey = if (page == response.totalPages) null else page + 1
            )
        } catch (exception: Exception) {
            LoadResult.Error(exception)
        }
    }

    override fun getRefreshKey(state: PagingState<Int, DataBook>): Int? {
        return state.anchorPosition?.let { anchorPosition ->
            // This loads starting from previous page, but since PagingConfig.initialLoadSize spans
            // multiple pages, the initial load will still load items centered around
            // anchorPosition. This also prevents needing to immediately launch prepend due to
            // prefetchDistance.
            state.closestPageToPosition(anchorPosition)?.prevKey
        }
    }

}