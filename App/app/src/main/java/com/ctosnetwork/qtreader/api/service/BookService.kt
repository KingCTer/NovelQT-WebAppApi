/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:56, 22/10/2021
 */

package com.ctosnetwork.qtreader.api.service

import com.ctosnetwork.qtreader.api.RetrofitClient
import com.ctosnetwork.qtreader.api.data.BookResponse
import retrofit2.http.GET
import retrofit2.http.Query

interface BookService {

    @GET("Book/pagination")
    suspend fun getBooks(
        @Query("page_number") pageNumber: Int,
        @Query("page_size") pageSize: Int
    ) : BookResponse

    companion object {

        fun create(): BookService {
            return RetrofitClient.getClient().create(BookService::class.java)
        }

    }

}