/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 20:04, 29/10/2021
 */

package com.ctosnetwork.qtreader.api.service

import com.ctosnetwork.qtreader.api.RetrofitClient
import com.ctosnetwork.qtreader.api.data.BookSearchResponse
import com.ctosnetwork.qtreader.api.data.ChapterSearchResponse
import retrofit2.http.GET
import retrofit2.http.Query

interface SearchService {

    @GET("Search/Book")
    suspend fun getBookSearch(
        @Query("page_number") pageNumber: Int,
        @Query("page_size") pageSize: Int,
        @Query("query") query: String
    ) : BookSearchResponse

    @GET("Search/Chapter")
    suspend fun getChapterSearch(
        @Query("page_number") pageNumber: Int,
        @Query("page_size") pageSize: Int,
        @Query("query") query: String
    ) : ChapterSearchResponse

    companion object {
        fun create(): SearchService {
            return RetrofitClient.getClient().create(SearchService::class.java)
        }
    }
}