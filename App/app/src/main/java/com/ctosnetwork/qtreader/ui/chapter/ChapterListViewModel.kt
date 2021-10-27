/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 15:44, 27/10/2021
 */

package com.ctosnetwork.qtreader.ui.chapter

import androidx.lifecycle.SavedStateHandle
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.PagingData
import androidx.paging.cachedIn
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.api.data.DataChapter
import com.ctosnetwork.qtreader.api.repository.ChapterRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

/**
 * The ViewModel used in [ChapterListFragment].
 */
@HiltViewModel
class ChapterListViewModel @Inject constructor(
    savedStateHandle: SavedStateHandle,
    private val repository: ChapterRepository
) : ViewModel() {

    val bookId: String = savedStateHandle.get<String>(BOOK_ID_SAVED_STATE_KEY)!!

    private var currentGetResult: Flow<PagingData<DataChapter>>? = null


    fun getChapterList(query: String): Flow<PagingData<DataChapter>> {
        val newResult: Flow<PagingData<DataChapter>> =
            repository.getChapterListResultStream(bookId, query).cachedIn(viewModelScope)
        currentGetResult = newResult
        return newResult
    }

    companion object {
        private const val BOOK_ID_SAVED_STATE_KEY = "bookId"
    }
}