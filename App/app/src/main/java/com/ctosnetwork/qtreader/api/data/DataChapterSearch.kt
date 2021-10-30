/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 19:57, 29/10/2021
 */

package com.ctosnetwork.qtreader.api.data

import com.google.gson.annotations.SerializedName

data class DataChapterSearch(
    @field:SerializedName("contentHighlight") val contentHighlight: List<String>,
    @field:SerializedName("bookResult") val bookResult: DataBook,
    @field:SerializedName("chapterResult") val chapterResult: DataChapter
)