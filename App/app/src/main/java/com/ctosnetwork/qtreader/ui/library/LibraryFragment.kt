/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 12:17, 22/10/2021
 */

package com.ctosnetwork.qtreader.ui.library

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.lifecycleScope
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.adapters.BookAdapter
import com.ctosnetwork.qtreader.adapters.ImageSliderAdapter
import com.ctosnetwork.qtreader.data.ImageSlider
import com.ctosnetwork.qtreader.databinding.FragmentLibraryBinding
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.Job
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

@AndroidEntryPoint
class LibraryFragment : Fragment() {

    private val libraryViewModel: LibraryViewModel by viewModels()

    private lateinit var sliderImageList: ArrayList<ImageSlider>
    private lateinit var sliderAdapter: ImageSliderAdapter

    private var getNewlyJob: Job? = null
    private var getFavoriteJob: Job? = null
    private var getPopularJob: Job? = null
    private val newlyAdapter = BookAdapter(BookAdapter.VIEW_TYPE_VERTICAL_CARD)
    private val favoriteAdapter = BookAdapter(BookAdapter.VIEW_TYPE_VERTICAL_CARD)
    private val popularAdapter = BookAdapter(BookAdapter.VIEW_TYPE_VERTICAL_CARD)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val binding = FragmentLibraryBinding.inflate(inflater, container, false)
        context ?: return binding.root

        requireActivity().setTheme(R.style.Theme_Library)

        val viewPager = binding.viewPagerImageSlider

        sliderImageList = ArrayList()
        sliderAdapter = ImageSliderAdapter()
        viewPager.adapter = sliderAdapter

        sliderImageList.add(
            ImageSlider(
                "1",
                "https://truyen.tangthuvien.vn/images/slide7.jpg",
                "1"
            )
        )
        sliderImageList.add(
            ImageSlider(
                "2",
                "https://truyen.tangthuvien.vn/images/slide9.jpg",
                "2"
            )
        )
        sliderImageList.add(
            ImageSlider(
                "3",
                "https://truyen.tangthuvien.vn/images/slide8.jpg",
                "3"
            )
        )

        sliderAdapter.submitList(sliderImageList)

        binding.dotsIndicator.setViewPager2(viewPager)

        lifecycleScope.launch {
            while (true) {
                for (i in 0..sliderImageList.size) {
                    delay(2500)
                    viewPager.setCurrentItem(i, true)
                }
            }
        }


        binding.recyclerViewNewly.adapter = newlyAdapter
        binding.recyclerViewFavorite.adapter = favoriteAdapter
        binding.recyclerViewPopular.adapter = popularAdapter

        getNewlyJob?.cancel()
        getNewlyJob = lifecycleScope.launch {
            libraryViewModel.getBooks("orderBy:key:desc").collectLatest {
                newlyAdapter.submitData(it)
            }
        }
        getFavoriteJob?.cancel()
        getFavoriteJob = lifecycleScope.launch {
            libraryViewModel.getBooks("orderBy:like:desc").collectLatest {
                favoriteAdapter.submitData(it)
            }
        }

        getPopularJob?.cancel()
        getPopularJob = lifecycleScope.launch {
            libraryViewModel.getBooks("orderBy:view:desc").collectLatest {
                popularAdapter.submitData(it)
            }
        }


        return binding.root
    }

}