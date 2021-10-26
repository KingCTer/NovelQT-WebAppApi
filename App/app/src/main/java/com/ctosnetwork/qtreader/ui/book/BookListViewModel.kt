/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 18:06, 24/10/2021
 */

package com.ctosnetwork.qtreader.ui.book

import androidx.lifecycle.SavedStateHandle
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.PagingData
import androidx.paging.cachedIn
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.api.repository.BookRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

/**
 * The ViewModel used in [BookListFragment].
 */
@HiltViewModel
class BookListViewModel @Inject constructor(
    savedStateHandle: SavedStateHandle,
    private val repository: BookRepository
) : ViewModel() {

    val toolbarTitle: String = savedStateHandle.get<String>(TOOLBAR_TITLE_SAVED_STATE_KEY)!!
    private val currentQueryValue: String = savedStateHandle.get<String>(QUERY_BOOK_SAVED_STATE_KEY)!!

    private var currentGetResult: Flow<PagingData<DataBook>>? = null

    fun getBooks(): Flow<PagingData<DataBook>> {
        val newResult: Flow<PagingData<DataBook>> =
            repository.getBookResultStream(currentQueryValue).cachedIn(viewModelScope)
        currentGetResult = newResult
        return newResult
    }


    companion object {
        private const val QUERY_BOOK_SAVED_STATE_KEY = "query"
        private const val TOOLBAR_TITLE_SAVED_STATE_KEY = "title"
    }
}