/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 15:44, 29/10/2021
 */

package com.ctosnetwork.qtreader.ui.book

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.PagingData
import androidx.paging.cachedIn
import com.ctosnetwork.qtreader.api.data.DataBookSearch
import com.ctosnetwork.qtreader.api.repository.SearchRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

/**
 * The ViewModel used in [BookSearchViewModel].
 */
@HiltViewModel
class BookSearchViewModel @Inject constructor(
    private val repository: SearchRepository
) : ViewModel() {

    private var currentQueryValue: String? = null
    private var currentGetResult: Flow<PagingData<DataBookSearch>>? = null

    fun searchBook(query: String): Flow<PagingData<DataBookSearch>> {
        currentQueryValue = query
        val newResult: Flow<PagingData<DataBookSearch>> =
            repository.getBookSearchResultStream(query).cachedIn(viewModelScope)
        currentGetResult = newResult
        return newResult
    }

}