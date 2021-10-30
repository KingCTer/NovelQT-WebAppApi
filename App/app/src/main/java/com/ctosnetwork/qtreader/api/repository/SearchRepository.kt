/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 20:08, 29/10/2021
 */

package com.ctosnetwork.qtreader.api.repository

import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.PagingData
import com.ctosnetwork.qtreader.api.data.DataBookSearch
import com.ctosnetwork.qtreader.api.data.DataChapterSearch
import com.ctosnetwork.qtreader.api.service.SearchService
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class SearchRepository @Inject constructor(private val service: SearchService) {

    fun getBookSearchResultStream(query: String): Flow<PagingData<DataBookSearch>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = NETWORK_PAGE_SIZE),
            pagingSourceFactory = { BookSearchPagingSource(service, query) }
        ).flow
    }

    fun getChapterSearchResultStream(query: String): Flow<PagingData<DataChapterSearch>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = NETWORK_PAGE_SIZE),
            pagingSourceFactory = { ChapterSearchPagingSource(service, query) }
        ).flow
    }

    companion object {
        const val NETWORK_PAGE_SIZE = 10
    }
}