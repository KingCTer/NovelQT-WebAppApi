/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:15, 27/10/2021
 */

package com.ctosnetwork.qtreader.api.data

import com.google.gson.annotations.SerializedName

data class DataChapter(
    @field:SerializedName("id") val id: String,
    @field:SerializedName("bookId") val bookId: String,
    @field:SerializedName("order") val order: Int,
    @field:SerializedName("name") val name: String,
    @field:SerializedName("url") val url: String,
    @field:SerializedName("content") val content: String
)