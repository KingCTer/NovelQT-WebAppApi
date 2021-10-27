/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 22:46, 27/10/2021
 */

package com.ctosnetwork.qtreader.utils

import android.view.View

object ViewAnimation {

    fun hideFab(fab: View) {
        val moveY = 2 * fab.height
        fab.animate()
            .translationY(moveY.toFloat())
            .setDuration(300)
            .start()
    }

    fun showFab(fab: View) {
        fab.animate()
            .translationY(0f)
            .setDuration(300)
            .start()
    }

}