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
 * The ViewModel used in [BookDetailFragment].
 */
@HiltViewModel
class BookDetailViewModel @Inject constructor(
    savedStateHandle: SavedStateHandle,
    private val repository: BookRepository
) : ViewModel() {

    val bookId: String = savedStateHandle.get<String>(BOOK_ID_SAVED_STATE_KEY)!!

    lateinit var book: DataBook

    lateinit var authorNameString: String
    lateinit var statusString: String
    lateinit var categoryNameString: String

    lateinit var likeString: String
    lateinit var viewString: String
    lateinit var chapterTotalString: String

    private var currentQueryValue: String? = null
    private var currentGetResult: Flow<PagingData<DataBook>>? = null

    fun getBooks(query: String): Flow<PagingData<DataBook>> {
        currentQueryValue = query
        val newResult: Flow<PagingData<DataBook>> =
            repository.getBookResultStream(query).cachedIn(viewModelScope)
        currentGetResult = newResult
        return newResult
    }

    companion object {
        private const val BOOK_ID_SAVED_STATE_KEY = "bookId"
    }

}