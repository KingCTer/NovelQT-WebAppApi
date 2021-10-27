/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 19:46, 27/10/2021
 */

package com.ctosnetwork.qtreader.ui.chapter

import androidx.lifecycle.SavedStateHandle
import androidx.lifecycle.ViewModel
import com.ctosnetwork.qtreader.api.data.DataChapter
import com.ctosnetwork.qtreader.api.repository.ChapterRepository
import dagger.hilt.android.lifecycle.HiltViewModel
import javax.inject.Inject

/**
 * The ViewModel used in [ChapterContentFragment].
 */
@HiltViewModel
class ChapterContentViewModel @Inject constructor(
    savedStateHandle: SavedStateHandle,
    val chapterRepository: ChapterRepository
) : ViewModel() {

    val chapterId: String = savedStateHandle.get<String>(CHAPTER_ID_SAVED_STATE_KEY)!!
    val bookId: String = savedStateHandle.get<String>(BOOK_ID_SAVED_STATE_KEY)!!
    val order: Int = savedStateHandle.get<Int>(ORDER_SAVED_STATE_KEY)!!

    var chapter: DataChapter? = null

    lateinit var chapterNameSplit: String

    companion object {
        private const val CHAPTER_ID_SAVED_STATE_KEY = "chapterId"
        private const val BOOK_ID_SAVED_STATE_KEY = "bookId"
        private const val ORDER_SAVED_STATE_KEY = "order"
    }

}