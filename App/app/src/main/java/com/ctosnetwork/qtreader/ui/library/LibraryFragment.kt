/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 12:17, 22/10/2021
 */

package com.ctosnetwork.qtreader.ui.library

import android.os.Bundle
import android.view.*
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.RecyclerView
import androidx.viewpager2.widget.ViewPager2
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.adapters.BookAdapter
import com.ctosnetwork.qtreader.adapters.ImageSliderAdapter
import com.ctosnetwork.qtreader.data.ImageSlider
import com.ctosnetwork.qtreader.databinding.FragmentLibraryBinding
import com.ctosnetwork.qtreader.utils.Tools
import com.google.android.material.bottomnavigation.BottomNavigationView
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.Job
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

private const val QUERY_BOOK_NEWLY = "orderBy:key:desc"
private const val QUERY_BOOK_FAVORITE = "orderBy:like:desc"
private const val QUERY_BOOK_POPULAR = "orderBy:view:desc"

@AndroidEntryPoint
class LibraryFragment : Fragment() {

    private lateinit var binding: FragmentLibraryBinding
    private val libraryViewModel: LibraryViewModel by viewModels()

    private var newlyJob: Job? = null
    private var favoriteJob: Job? = null
    private var popularJob: Job? = null
    private val newlyAdapter = BookAdapter(BookAdapter.VIEW_TYPE_VERTICAL_CARD)
    private val favoriteAdapter = BookAdapter(BookAdapter.VIEW_TYPE_VERTICAL_CARD)
    private val popularAdapter = BookAdapter(BookAdapter.VIEW_TYPE_HORIZONTAL_CARD)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentLibraryBinding.inflate(inflater, container, false)
        context ?: return binding.root

        Tools.setSystemBarColor(requireActivity(), R.color.transparent)
        Tools.showBottomNavigationView(requireActivity())

        binding.setClickSearchBookListener {
            val direction =
                LibraryFragmentDirections.actionNavigationLibraryToBookSearchFragment()
            findNavController().navigate(direction)
        }

        binding.setClickSearchChapterListener {
            val direction =
                LibraryFragmentDirections.actionNavigationLibraryToChapterSearchFragment()
            findNavController().navigate(direction)
        }

        initialImageSlider(binding.viewPagerImageSlider)
        newlyJob = initialBook(binding.recyclerViewNewly, newlyAdapter, newlyJob, QUERY_BOOK_NEWLY)
        favoriteJob = initialBook(
            binding.recyclerViewFavorite,
            favoriteAdapter,
            favoriteJob,
            QUERY_BOOK_FAVORITE
        )
        popularJob =
            initialBook(binding.recyclerViewPopular, popularAdapter, popularJob, QUERY_BOOK_POPULAR)

        setListClick(binding.newlyList, QUERY_BOOK_NEWLY, "Truyện mới cập nhật")
        setListClick(binding.favoriteList, QUERY_BOOK_FAVORITE, "Truyện được yêu thích")
        setListClick(binding.popularList, QUERY_BOOK_POPULAR, "Truyện phổ biến")

        return binding.root
    }

    private fun setListClick(view: TextView, query: String, title: String) {
        view.setOnClickListener {
            val direction =
                LibraryFragmentDirections.actionNavigationLibraryToBookListFragment(query, title)
            findNavController().navigate(direction)
        }
    }

    private fun initialBook(
        recyclerView: RecyclerView,
        bookAdapter: BookAdapter,
        job: Job?,
        query: String
    ): Job {
        recyclerView.adapter = bookAdapter
        if (job != null) return job
        job?.cancel()
        val newJob = lifecycleScope.launch {
            libraryViewModel.getBooks(query).collectLatest {
                bookAdapter.submitData(it)
            }
        }
        return newJob

    }

    private fun initialImageSlider(viewPager: ViewPager2) {

        val sliderImageList: ArrayList<ImageSlider> = ArrayList()
        val sliderAdapter: ImageSliderAdapter = ImageSliderAdapter()

        viewPager.adapter = sliderAdapter

        sliderImageList.add(
            ImageSlider(
                "1",
                "https://truyen.tangthuvien.vn/images/slide7.jpg",
                "6029768f-1a09-464f-8a83-55cddb46c32b"
            )
        )
        sliderImageList.add(
            ImageSlider(
                "2",
                "https://truyen.tangthuvien.vn/images/slide9.jpg",
                "7d997271-9784-433e-a1cf-bc02161969d0"
            )
        )
        sliderImageList.add(
            ImageSlider(
                "3",
                "https://truyen.tangthuvien.vn/images/slide8.jpg",
                "a855e969-987a-4c7a-a63b-15506b0fe8c0"
            )
        )

        sliderAdapter.submitList(sliderImageList)

        binding.dotsIndicator.setViewPager2(viewPager)

        lifecycleScope.launch {
            while (true) {
                delay(2500)
                if (viewPager.currentItem == sliderImageList.size - 1) {
                    viewPager.setCurrentItem(0, true)
                } else {
                    viewPager.setCurrentItem(viewPager.currentItem + 1, true)
                }
            }
        }
    }


}