/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:11, 27/10/2021
 */

package com.ctosnetwork.qtreader.api.service

import com.ctosnetwork.qtreader.api.RetrofitClient
import com.ctosnetwork.qtreader.api.data.BookResponse
import com.ctosnetwork.qtreader.api.data.BooksResponse
import com.ctosnetwork.qtreader.api.data.ChapterResponse
import com.ctosnetwork.qtreader.api.data.ChaptersResponse
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query

interface ChapterService {

    @GET("Chapter/pagination/{bookId}")
    suspend fun getChapterList(
        @Path("bookId") bookId: String,
        @Query("page_number") pageNumber: Int,
        @Query("page_size") pageSize: Int,
        @Query("query") query: String
    ) : ChaptersResponse

    @GET("Chapter/{id}")
    fun getChapter(
        @Path("id") id: String
    ) : Call<ChapterResponse>

    @GET("Chapter/{bookId}/{order}")
    fun getChapterByOrder(
        @Path("bookId") bookId: String,
        @Path("order") order: Int
    ) : Call<ChapterResponse>

    @GET("Chapter/last/{bookId}")
    fun getLastChapter(
        @Path("bookId") bookId: String
    ) : Call<ChapterResponse>

    companion object {
        fun create(): ChapterService {
            return RetrofitClient.getClient().create(ChapterService::class.java)
        }
    }

}