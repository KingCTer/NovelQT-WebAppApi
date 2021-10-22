/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 19:17, 21/09/2021
 */

package com.ctosnetwork.qtreader.ui.onboarding

import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentActivity
import androidx.viewpager2.adapter.FragmentStateAdapter

class OnBoardingAdapter(
    fa: FragmentActivity,
    listFragment: ArrayList<Fragment>
): FragmentStateAdapter(fa) {

    private var fragmentList :ArrayList<Fragment> = listFragment

    override fun getItemCount(): Int = fragmentList.size

    override fun createFragment(position: Int): Fragment = fragmentList[position]

}