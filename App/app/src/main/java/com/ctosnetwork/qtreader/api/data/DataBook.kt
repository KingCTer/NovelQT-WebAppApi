/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by MyLaptop on 21:31, 21/10/2021
 */

package com.ctosnetwork.qtreader.api.data

import com.google.gson.annotations.SerializedName

data class DataBook(
    @field:SerializedName("id") val id: String,
    @field:SerializedName("name") val name: String,
    @field:SerializedName("key") val key: String,
    @field:SerializedName("cover") val cover: String,
    @field:SerializedName("status") val status: String,
    @field:SerializedName("authorId") val authorId: String,
    @field:SerializedName("categoryId") val categoryId: String,
    @field:SerializedName("indexStatus") val indexStatus: Int,
    @field:SerializedName("like") val like: Int,
    @field:SerializedName("view") val view: Int
)