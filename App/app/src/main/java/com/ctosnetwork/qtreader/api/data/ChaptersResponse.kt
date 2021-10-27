/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:15, 27/10/2021
 */

package com.ctosnetwork.qtreader.api.data

import com.google.gson.annotations.SerializedName

data class ChaptersResponse(
    @field: SerializedName("success") val success: Boolean,
    @field: SerializedName("message") val message: String,
    @field: SerializedName("pageNumber") val pageNumber: Int,
    @field: SerializedName("pageSize") val pageSize: Int,
    @field: SerializedName("totalPages") val totalPages: Int,
    @field: SerializedName("totalRecords") val totalRecords: Int,
    @field: SerializedName("data") val data: List<DataChapter>
)