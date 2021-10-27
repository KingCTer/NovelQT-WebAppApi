/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 22:33, 27/10/2021
 */

package com.ctosnetwork.qtreader.utils

import android.annotation.SuppressLint
import android.app.Activity
import android.app.Dialog
import android.content.Context
import android.graphics.Color
import android.os.Build
import android.view.View
import android.view.WindowManager
import androidx.annotation.ColorInt
import androidx.annotation.ColorRes
import androidx.appcompat.widget.Toolbar
import androidx.core.content.ContextCompat
import androidx.navigation.findNavController
import com.ctosnetwork.qtreader.R

object Tools {

    @SuppressLint("ObsoleteSdkInt")
    fun setSystemBarColor(act: Activity) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            val window = act.window
            window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
            window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
            window.statusBarColor = ContextCompat.getColor(act, R.color.primaryColor)
        }
    }

    @SuppressLint("ObsoleteSdkInt")
    fun setSystemBarColor(act: Activity, @ColorRes color: Int) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            val window = act.window
            window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
            window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
            window.statusBarColor = ContextCompat.getColor(act, color)
        }
    }

    @SuppressLint("ObsoleteSdkInt")
    fun setSystemBarColorInt(act: Activity, @ColorInt color: Int) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            val window = act.window
            window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
            window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
            window.statusBarColor = color
        }
    }

    @SuppressLint("ObsoleteSdkInt")
    fun setSystemBarColorDialog(act: Context, dialog: Dialog, @ColorRes color: Int) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            val window = dialog.window
            window!!.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
            window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
            window.statusBarColor = ContextCompat.getColor(act, color)
        }
    }

    @SuppressLint("ObsoleteSdkInt")
    fun setSystemBarLight(act: Activity) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            val view = act.findViewById<View>(R.id.content)
            var flags = view.systemUiVisibility
            flags = flags or View.SYSTEM_UI_FLAG_LIGHT_STATUS_BAR
            view.systemUiVisibility = flags
        }
    }

    @SuppressLint("ObsoleteSdkInt")
    fun setSystemBarLightDialog(dialog: Dialog) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            val view = dialog.findViewById<View>(R.id.content)
            var flags = view.systemUiVisibility
            flags = flags or View.SYSTEM_UI_FLAG_LIGHT_STATUS_BAR
            view.systemUiVisibility = flags
        }
    }

    @SuppressLint("ObsoleteSdkInt")
    fun clearSystemBarLight(act: Activity) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            val window = act.window
            window.statusBarColor = ContextCompat.getColor(act, R.color.primaryColor)
        }
    }

    /**
     * Making notification bar transparent
     */
    @SuppressLint("ObsoleteSdkInt")
    fun setSystemBarTransparent(act: Activity) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            val window = act.window
            window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
            window.statusBarColor = Color.TRANSPARENT
        }
    }

    fun setToolbarNavigateUp(toolBar: Toolbar) {
        toolBar.setNavigationOnClickListener { view ->
            view.findNavController().navigateUp()
        }
    }

}