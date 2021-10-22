/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:51, 22/10/2021
 */

package com.ctosnetwork.qtreader.api.repository

import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.PagingData
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.api.service.BookService
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class BookRepository @Inject constructor(private val service: BookService) {

    fun getBookResultStream(): Flow<PagingData<DataBook>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = NETWORK_PAGE_SIZE),
            pagingSourceFactory = { BookPagingSource(service) }
        ).flow
    }

    companion object {
        private const val NETWORK_PAGE_SIZE = 5
    }

}