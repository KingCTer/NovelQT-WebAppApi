/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by MyLaptop on 21:31, 21/10/2021
 */

package com.ctosnetwork.qtreader.api.data

import com.google.gson.annotations.SerializedName

data class BookResponse(
    @field: SerializedName("success") val success: Boolean,
    @field: SerializedName("totalRecords") val totalRecords: Int,
    @field: SerializedName("data") val data: DataBook
)