/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 18:06, 24/10/2021
 */

package com.ctosnetwork.qtreader.ui.book

import android.os.Bundle
import android.view.*
import androidx.core.content.ContextCompat
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.findNavController
import androidx.paging.PagingData
import androidx.recyclerview.widget.DividerItemDecoration
import androidx.recyclerview.widget.RecyclerView
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.adapters.BookAdapter
import com.ctosnetwork.qtreader.databinding.FragmentBookListBinding
import com.google.android.material.bottomnavigation.BottomNavigationView
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

@AndroidEntryPoint
class BookListFragment : Fragment() {

    private val bookListviewModel: BookListViewModel by viewModels()

    private var getBookListJob: Job? = null
    private val bookListAdapter = BookAdapter(BookAdapter.VIEW_TYPE_HORIZONTAL_CARD_LIST)

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val binding = DataBindingUtil.inflate<FragmentBookListBinding>(inflater, R.layout.fragment_book_list, container, false)

        requireActivity().window.statusBarColor = ContextCompat.getColor(requireActivity(), R.color.primaryColor)

        binding.viewModel = bookListviewModel
        binding.lifecycleOwner = viewLifecycleOwner

        binding.toolbarBookList.setNavigationOnClickListener { view ->
            view.findNavController().navigateUp()
        }

        getBookListJob = initialBook(binding.recyclerBookList, bookListAdapter, getBookListJob)

        binding.swipeBookList.setOnRefreshListener {
            bookListAdapter.submitData(lifecycle, PagingData.empty())
            getBookListJob = null
            getBookListJob = initialBook(binding.recyclerBookList, bookListAdapter, getBookListJob)
            binding.swipeBookList.isRefreshing = false
        }

        return binding.root
    }

    private fun initialBook(
        recyclerView: RecyclerView,
        bookAdapter: BookAdapter,
        job: Job?
    ): Job {
        recyclerView.adapter = bookAdapter
        recyclerView.addItemDecoration(DividerItemDecoration(requireContext(), DividerItemDecoration.VERTICAL))
        if (job != null) return job
        job?.cancel()
        val newJob = lifecycleScope.launch {
            bookListviewModel.getBooks().collectLatest {
                bookAdapter.submitData(it)
            }
        }
        return newJob

    }

}