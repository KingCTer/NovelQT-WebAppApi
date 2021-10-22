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
    private var _binding: FragmentLibraryBinding? = null

    private lateinit var sliderImageList: ArrayList<ImageSlider>
    private lateinit var sliderAdapter: ImageSliderAdapter

    private var getBookJob: Job? = null
    private val adapter = BookAdapter(BookAdapter.VIEW_TYPE_VERTICAL_CARD)

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentLibraryBinding.inflate(inflater, container, false)
        val root: View = binding.root

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


        binding.recyclerViewNewlyUpdate.adapter = adapter
        getBookJob?.cancel()
        getBookJob = lifecycleScope.launch {
            libraryViewModel.getBooks().collectLatest {
                adapter.submitData(it)
            }
        }


        return root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}