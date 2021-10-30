/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 10:32, 30/10/2021
 */

package com.ctosnetwork.qtreader.ui.chapter

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.PagingData
import androidx.paging.cachedIn
import com.ctosnetwork.qtreader.api.data.DataChapterSearch
import com.ctosnetwork.qtreader.api.repository.SearchRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

/**
 * The ViewModel used in [ChapterSearchViewModel].
 */
@HiltViewModel
class ChapterSearchViewModel @Inject constructor(
    private val repository: SearchRepository
) : ViewModel() {

    private var currentQueryValue: String? = null
    private var currentGetResult: Flow<PagingData<DataChapterSearch>>? = null

    fun searchBook(query: String): Flow<PagingData<DataChapterSearch>> {
        currentQueryValue = query
        val newResult: Flow<PagingData<DataChapterSearch>> =
            repository.getChapterSearchResultStream(query).cachedIn(viewModelScope)
        currentGetResult = newResult
        return newResult
    }

}