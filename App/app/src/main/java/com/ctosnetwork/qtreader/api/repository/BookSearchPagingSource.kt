/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 20:13, 29/10/2021
 */

package com.ctosnetwork.qtreader.api.repository

import androidx.paging.PagingSource
import androidx.paging.PagingState
import com.ctosnetwork.qtreader.api.data.DataBookSearch
import com.ctosnetwork.qtreader.api.service.SearchService

private const val STARTING_PAGE_INDEX = 1
private const val NETWORK_PAGE_SIZE = SearchRepository.NETWORK_PAGE_SIZE

class BookSearchPagingSource (
    private val service: SearchService,
    private val query: String
) : PagingSource<Int, DataBookSearch>() {

    override suspend fun load(params: LoadParams<Int>): LoadResult<Int, DataBookSearch> {
        val pageNumber = params.key ?: STARTING_PAGE_INDEX
        return try {
            val response = service.getBookSearch(pageNumber, NETWORK_PAGE_SIZE, query)
            val data = response.data
            LoadResult.Page(
                data = data,
                prevKey = if (pageNumber == STARTING_PAGE_INDEX) null else pageNumber - 1,
                nextKey = if (pageNumber == response.totalPages || response.totalPages == 0) null else pageNumber + 1
            )
        } catch (exception: Exception) {
            LoadResult.Error(exception)
        }
    }

    override fun getRefreshKey(state: PagingState<Int, DataBookSearch>): Int? {
        return state.anchorPosition?.let { anchorPosition ->
            // This loads starting from previous page, but since PagingConfig.initialLoadSize spans
            // multiple pages, the initial load will still load items centered around
            // anchorPosition. This also prevents needing to immediately launch prepend due to
            // prefetchDistance.
            state.closestPageToPosition(anchorPosition)?.prevKey
        }
    }
}