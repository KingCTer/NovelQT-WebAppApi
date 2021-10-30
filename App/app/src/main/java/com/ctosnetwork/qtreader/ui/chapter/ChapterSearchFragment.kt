/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 10:24, 30/10/2021
 */

package com.ctosnetwork.qtreader.ui.chapter

import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.inputmethod.EditorInfo
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.paging.PagingData
import androidx.recyclerview.widget.DividerItemDecoration
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.adapters.ChapterSearchAdapter
import com.ctosnetwork.qtreader.databinding.FragmentChapterSearchBinding
import com.ctosnetwork.qtreader.utils.Tools
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

@AndroidEntryPoint
class ChapterSearchFragment : Fragment() {

    private lateinit var binding: FragmentChapterSearchBinding
    private val viewModel: ChapterSearchViewModel by viewModels()

    private var searchJob: Job? = null
    private val adapter = ChapterSearchAdapter(ChapterSearchAdapter.VIEW_TYPE_HORIZONTAL_CARD_LIST)

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentChapterSearchBinding.inflate(inflater, container, false)
        context ?: return binding.root

        Tools.setSystemBarColor(requireActivity(), R.color.grey_5)
        Tools.showBottomNavigationView(requireActivity(), false)
        Tools.setToolbarNavigateUp(binding.chapterSearchToolbar)

        initSearchBar()

        binding.recyclerChapterSearch.adapter = adapter
        binding.recyclerChapterSearch.addItemDecoration(DividerItemDecoration(requireContext(), DividerItemDecoration.VERTICAL))

        return binding.root
    }

    private fun initSearchBar() {
        binding.chapterSearchQuery.addTextChangedListener(object : TextWatcher {
            override fun onTextChanged(c: CharSequence, i: Int, i1: Int, i2: Int) {
                if (c.toString().trim { it <= ' ' }.isEmpty()) {
                    binding.chapterSearchClear.visibility = View.GONE
                } else {
                    binding.chapterSearchClear.visibility = View.VISIBLE
                }
            }

            override fun beforeTextChanged(c: CharSequence, i: Int, i1: Int, i2: Int) {}
            override fun afterTextChanged(editable: Editable) {}
        })

        binding.chapterSearchQuery.setOnEditorActionListener(TextView.OnEditorActionListener { _, actionId, _ ->
            if (actionId == EditorInfo.IME_ACTION_SEARCH) {
                Tools.hideKeyboard(requireActivity())
                searchAction(binding.chapterSearchQuery.text.toString())
                return@OnEditorActionListener true
            }
            false
        })
    }

    private fun searchAction(query: String) {
        // Make sure we cancel the previous job before creating a new one
        adapter.submitData(lifecycle, PagingData.empty())
        searchJob = null
        searchJob?.cancel()
        searchJob = lifecycleScope.launch {
            viewModel.searchBook(query).collectLatest {
                adapter.submitData(it)
            }
        }

    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        Tools.showKeyboard(requireActivity())

        binding.chapterSearchClear.setOnClickListener {
            binding.chapterSearchQuery.setText("")
        }
    }

}