/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:03, 22/10/2021
 */

package com.ctosnetwork.qtreader.ui.library

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.PagingData
import androidx.paging.cachedIn
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.api.repository.BookRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

@HiltViewModel
class LibraryViewModel @Inject constructor(
    private val repository: BookRepository
) : ViewModel() {
    private var currentQueryValue: String? = null
    private var currentGetResult: Flow<PagingData<DataBook>>? = null

    fun getBooks(query: String): Flow<PagingData<DataBook>> {
        currentQueryValue = query
        val newResult: Flow<PagingData<DataBook>> =
            repository.getBookResultStream(query).cachedIn(viewModelScope)
        currentGetResult = newResult
        return newResult
    }
}