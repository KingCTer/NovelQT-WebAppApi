/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 14:05, 27/10/2021
 */

package com.ctosnetwork.qtreader.ui.chapter

import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.core.widget.doOnTextChanged
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.findNavController
import androidx.paging.PagingData
import androidx.recyclerview.widget.DividerItemDecoration
import androidx.recyclerview.widget.RecyclerView
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.adapters.BookAdapter
import com.ctosnetwork.qtreader.adapters.ChapterAdapter
import com.ctosnetwork.qtreader.databinding.FragmentChapterListBinding
import com.google.android.material.bottomnavigation.BottomNavigationView
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

@AndroidEntryPoint
class ChapterListFragment : Fragment() {

    private val viewModel: ChapterListViewModel by viewModels()

    private var getChapterListJob: Job? = null
    private val chapterListAdapter = ChapterAdapter(ChapterAdapter.VIEW_TYPE_HORIZONTAL_CARD_LIST)

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val binding = DataBindingUtil.inflate<FragmentChapterListBinding>(inflater, R.layout.fragment_chapter_list, container, false)

        requireActivity().window.statusBarColor = ContextCompat.getColor(requireActivity(), R.color.primaryColor)
        try {
            val navView = requireActivity().findViewById<BottomNavigationView>(R.id.nav_view)
            if (navView.visibility == View.VISIBLE){
                navView.visibility = View.GONE
            }
        } catch (e: Exception) {
            // handler
        }

        binding.viewModel = viewModel
        binding.lifecycleOwner = viewLifecycleOwner

        binding.toolbarChapterList.setNavigationOnClickListener { view ->
            view.findNavController().navigateUp()
        }

        getChapterListJob = initialChapterList(binding.recyclerChapterList, chapterListAdapter, getChapterListJob)

        var isFab: Boolean = false
        var currentOrderBy = "orderBy:order:asc;"
        binding.chapterListFab.setOnClickListener {
            if (isFab) {
                currentOrderBy = "orderBy:order:asc;"
                isFab = false
            } else {
                currentOrderBy = "orderBy:order:desc;"
                isFab = true
            }

            chapterListAdapter.submitData(lifecycle, PagingData.empty())
            getChapterListJob = null
            getChapterListJob = initialChapterList(binding.recyclerChapterList, chapterListAdapter, getChapterListJob, currentOrderBy)
        }
        // Get input text
        binding.searchChapterTextField.editText?.doOnTextChanged { inputText, _, _, _ ->
            val query = "where:order>" + inputText.toString()
            chapterListAdapter.submitData(lifecycle, PagingData.empty())
            getChapterListJob = null
            getChapterListJob = initialChapterList(binding.recyclerChapterList, chapterListAdapter, getChapterListJob, query)
        }

        binding.swipeChapterList.setOnRefreshListener {
            val query = "where:order>" + binding.searchChapterTextField.editText?.text.toString()
            chapterListAdapter.submitData(lifecycle, PagingData.empty())
            getChapterListJob = null
            getChapterListJob = initialChapterList(binding.recyclerChapterList, chapterListAdapter, getChapterListJob, query)
            binding.swipeChapterList.isRefreshing = false
        }





        return binding.root
    }

    private fun initialChapterList(
        recyclerView: RecyclerView,
        adapter: ChapterAdapter,
        job: Job?,
        query: String = ""
    ): Job {
        recyclerView.adapter = adapter
        recyclerView.addItemDecoration(DividerItemDecoration(requireContext(), DividerItemDecoration.VERTICAL))
        if (job != null) return job
        job?.cancel()
        val newJob = lifecycleScope.launch {
            viewModel.getChapterList(query).collectLatest {
                adapter.submitData(it)

            }
        }
        return newJob

    }

}