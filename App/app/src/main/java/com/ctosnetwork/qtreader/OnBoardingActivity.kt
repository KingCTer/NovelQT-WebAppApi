/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 12:19, 22/10/2021
 */

package com.ctosnetwork.qtreader

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.viewpager2.widget.ViewPager2
import com.ctosnetwork.qtreader.databinding.ActivityOnBoardingBinding
import com.ctosnetwork.qtreader.ui.onboarding.OnBoardingAdapter
import com.ctosnetwork.qtreader.ui.onboarding.OnBoardingFirstScreenFragment
import com.ctosnetwork.qtreader.ui.onboarding.OnBoardingSecondScreenFragment
import com.ctosnetwork.qtreader.ui.onboarding.OnBoardingThirdScreenFragment

class OnBoardingActivity : AppCompatActivity() {

    private lateinit var binding: ActivityOnBoardingBinding

    private lateinit var viewPager: ViewPager2

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityOnBoardingBinding.inflate(layoutInflater)

        // Hooks
        val dotsIndicator = binding.dotsIndicator
        val viewPager = binding.viewPagerOnboarding

        setContentView(binding.root)

        val fragmentList = arrayListOf(
            OnBoardingFirstScreenFragment(),
            OnBoardingSecondScreenFragment(),
            OnBoardingThirdScreenFragment()
        )
        val viewPagerAdapter = OnBoardingAdapter(this, fragmentList)

        viewPager.adapter = viewPagerAdapter
        //viewPager.setPageTransformer(DepthPageTransformer())

        dotsIndicator.setViewPager2(viewPager)

    }
}