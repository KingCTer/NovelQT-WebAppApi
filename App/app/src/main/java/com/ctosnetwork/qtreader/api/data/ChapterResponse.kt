/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:20, 27/10/2021
 */

package com.ctosnetwork.qtreader.api.data

import com.google.gson.annotations.SerializedName

data class ChapterResponse(
    @field: SerializedName("success") val success: Boolean,
    @field: SerializedName("totalRecords") val totalRecords: Int,
    @field: SerializedName("data") val data: DataChapter
)