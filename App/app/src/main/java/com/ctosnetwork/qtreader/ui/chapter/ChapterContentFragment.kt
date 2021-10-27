/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 19:42, 27/10/2021
 */

package com.ctosnetwork.qtreader.ui.chapter

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.navigation.fragment.findNavController
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.api.data.DataChapter
import com.ctosnetwork.qtreader.databinding.FragmentChapterContentBinding
import com.ctosnetwork.qtreader.utils.Tools
import com.ctosnetwork.qtreader.utils.ViewAnimation
import com.google.android.material.appbar.AppBarLayout
import dagger.hilt.android.AndroidEntryPoint
import kotlin.math.abs

@AndroidEntryPoint
class ChapterContentFragment : Fragment() {

    private lateinit var binding: FragmentChapterContentBinding
    private val viewModel: ChapterContentViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentChapterContentBinding.inflate(inflater, container, false)
        context ?: return binding.root

        Tools.setSystemBarColor(requireActivity())
        Tools.setToolbarNavigateUp(binding.chapterContentToolbar)

        initAppbar()
        initToolbar()



        if (viewModel.chapterId == "") {
            getChapterByOrder()
        } else {
            getChapterById()
        }

        return binding.root
    }




    private fun getChapterById() {
        viewModel.chapterRepository.getChapterById(
            viewModel.chapterId
        ) { chapter: DataChapter ->
            initChapter(chapter)
        }
    }

    private fun getChapterByOrder() {
        viewModel.chapterRepository.getChapterByOrder(
            viewModel.bookId,
            viewModel.order
        ) { chapter: DataChapter? ->
            if (chapter != null) {
                initChapter(chapter)
            }
        }
    }

    private fun initChapter(chapter: DataChapter) {
        viewModel.chapter = chapter

        viewModel.chapterNameSplit = chapter.name.split(":")[0]

        binding.viewModel = viewModel
        initFab()
    }

    private fun initAppbar() {
        val appBar = binding.chapterContentAppBar
        var isCollapsed: Boolean = false
        appBar.addOnOffsetChangedListener(AppBarLayout.OnOffsetChangedListener { _, verticalOffset ->
            //Check if the view is collapsed
            if (abs(verticalOffset) >= appBar.totalScrollRange) {
                Tools.setSystemBarColor(requireActivity(), R.color.md_amber_50)
                ViewAnimation.hideFab(binding.chapterContentFabNext)
                ViewAnimation.hideFab(binding.chapterContentFabBack)
                isCollapsed = true
            } else if (isCollapsed) {
                Tools.setSystemBarColor(requireActivity())
                ViewAnimation.showFab(binding.chapterContentFabNext)
                ViewAnimation.showFab(binding.chapterContentFabBack)
            }
        })
    }

    private fun initToolbar() {
        binding.chapterContentToolbar.setOnMenuItemClickListener { menuItem ->
            when (menuItem.itemId) {
                R.id.chapter_content_menu_next_chapter -> {
                    nextChapter()
                    true
                }
                R.id.chapter_content_menu_list_chapter -> {
                    val direction =
                        ChapterContentFragmentDirections.actionChapterContentFragmentToChapterListFragment(
                            bookId = viewModel.bookId
                        )
                    findNavController().navigate(direction)
                    true
                }
                else -> false
            }
        }
    }

    private fun initFab() {
        binding.chapterContentFabNext.setOnClickListener {
            nextChapter()
        }
        if (viewModel.order > 1) {
            binding.chapterContentFabBack.setOnClickListener {
                previousChapter()
            }

        }
    }

    private fun previousChapter() {
        val direction =
            ChapterContentFragmentDirections.actionChapterContentFragmentSelf(
                bookId = viewModel.bookId,
                order = viewModel.order - 1,
                chapterId = ""
            )
        findNavController().navigate(direction)
    }

    private fun nextChapter() {
        val direction =
            ChapterContentFragmentDirections.actionChapterContentFragmentSelf(
                bookId = viewModel.bookId,
                order = viewModel.order + 1,
                chapterId = ""
            )
        findNavController().navigate(direction)
    }

}