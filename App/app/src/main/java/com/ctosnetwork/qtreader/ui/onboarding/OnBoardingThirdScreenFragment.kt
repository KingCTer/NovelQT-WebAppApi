/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 12:38, 22/10/2021
 */

package com.ctosnetwork.qtreader.ui.onboarding

import android.content.Context
import android.content.Intent
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.viewpager2.widget.ViewPager2
import com.ctosnetwork.qtreader.MainActivity
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.databinding.FragmentOnBoardingFirstScreenBinding
import com.ctosnetwork.qtreader.databinding.FragmentOnBoardingThirdScreenBinding

class OnBoardingThirdScreenFragment : Fragment() {

    private lateinit var binding: FragmentOnBoardingThirdScreenBinding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentOnBoardingThirdScreenBinding.inflate(inflater, container, false)

        binding.textViewFinish.setOnClickListener{
            onBoardingFinished()
            requireActivity().run {
                startActivity(Intent(this, MainActivity::class.java))
                finish()
            }
        }

        return binding.root
    }

    private fun onBoardingFinished(){
        val sharedPref = requireActivity().getSharedPreferences("onBoarding", Context.MODE_PRIVATE)
        val editor = sharedPref.edit()
        editor.putBoolean("Finished", true)
        editor.apply()
    }

}