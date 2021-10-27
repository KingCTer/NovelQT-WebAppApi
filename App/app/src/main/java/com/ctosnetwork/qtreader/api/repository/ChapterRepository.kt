/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:25, 27/10/2021
 */

package com.ctosnetwork.qtreader.api.repository

import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.PagingData
import com.ctosnetwork.qtreader.api.data.BookResponse
import com.ctosnetwork.qtreader.api.data.ChapterResponse
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.api.data.DataChapter
import com.ctosnetwork.qtreader.api.service.ChapterService
import kotlinx.coroutines.flow.Flow
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import javax.inject.Inject

class ChapterRepository @Inject constructor(private val service: ChapterService) {

    companion object {
        private const val NETWORK_PAGE_SIZE = 200
    }

    fun getChapterListResultStream(bookId: String, query: String): Flow<PagingData<DataChapter>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = NETWORK_PAGE_SIZE),
            pagingSourceFactory = { ChapterPagingSource(service, bookId, query) }
        ).flow
    }

    fun getChapterById(id: String, getBook: (result: DataChapter) -> Unit) {

        service.getChapter(id).enqueue(object : Callback<ChapterResponse> {
            override fun onResponse(
                call: Call<ChapterResponse>,
                response: Response<ChapterResponse>
            ) {
                if (response.isSuccessful) {
                    getBook.invoke(response.body()!!.data)
                }
            }

            override fun onFailure(call: Call<ChapterResponse>, t: Throwable) {
            }

        })
    }

    fun getChapterByOrder(bookId: String, order: Int, getBook: (result: DataChapter?) -> Unit) {

        service.getChapterByOrder(bookId, order).enqueue(object : Callback<ChapterResponse> {
            override fun onResponse(
                call: Call<ChapterResponse>,
                response: Response<ChapterResponse>
            ) {
                if (response.isSuccessful) {
                    getBook.invoke(response.body()?.data)
                }
            }

            override fun onFailure(call: Call<ChapterResponse>, t: Throwable) {
            }

        })
    }

    fun getLastChapter(bookId: String, getBook: (result: DataChapter) -> Unit) {

        service.getLastChapter(bookId).enqueue(object : Callback<ChapterResponse> {
            override fun onResponse(
                call: Call<ChapterResponse>,
                response: Response<ChapterResponse>
            ) {
                if (response.isSuccessful) {
                    getBook.invoke(response.body()!!.data)
                }
            }

            override fun onFailure(call: Call<ChapterResponse>, t: Throwable) {
            }

        })
    }

}