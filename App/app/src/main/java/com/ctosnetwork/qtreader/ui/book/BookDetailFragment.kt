/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 18:07, 24/10/2021
 */

package com.ctosnetwork.qtreader.ui.book

import android.os.Bundle
import android.text.Html
import android.view.*
import android.view.View.*
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.findNavController
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.bumptech.glide.request.RequestOptions
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.adapters.BookAdapter
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.api.data.DataChapter
import com.ctosnetwork.qtreader.api.repository.BookRepository
import com.ctosnetwork.qtreader.api.repository.ChapterRepository
import com.ctosnetwork.qtreader.api.service.BookService
import com.ctosnetwork.qtreader.api.service.ChapterService
import com.ctosnetwork.qtreader.databinding.FragmentBookDetailBinding
import com.google.android.material.appbar.AppBarLayout
import com.google.android.material.appbar.AppBarLayout.OnOffsetChangedListener
import com.google.android.material.bottomnavigation.BottomNavigationView
import dagger.hilt.android.AndroidEntryPoint
import jp.wasabeef.glide.transformations.BlurTransformation
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch
import kotlin.math.abs


@AndroidEntryPoint
class BookDetailFragment : Fragment() {

    private lateinit var binding: FragmentBookDetailBinding
    private val bookDetailViewModel: BookDetailViewModel by viewModels()

    private var getSameAuthorJob: Job? = null
    private val sameAuthorAdapter = BookAdapter(BookAdapter.VIEW_TYPE_VERTICAL_CARD)

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        //val binding = DataBindingUtil.inflate<FragmentBookDetailBinding>(inflater, R.layout.fragment_book_detail, container,false)
        binding = FragmentBookDetailBinding.inflate(inflater, container, false)
        context ?: return binding.root

        try {
            val navView = requireActivity().findViewById<BottomNavigationView>(R.id.nav_view)
            if (navView.visibility == View.VISIBLE){
                navView.visibility = View.GONE
            }
        } catch (e: Exception) {
            // handler
        }

        val window: Window = requireActivity().window
        window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
        window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
        window.statusBarColor = ContextCompat.getColor(requireActivity(), R.color.transparent)

        binding.bookDetailToolbar.setNavigationOnClickListener { view ->
            view.findNavController().navigateUp()
        }

        val repository = BookRepository(BookService.create())
        repository.getBookById(bookDetailViewModel.bookId){ result: DataBook? ->
            val book = result!!
            bookDetailViewModel.book = book
            bookDetailViewModel.authorNameString = "Tác giả: " + book.authorName
            bookDetailViewModel.statusString = "Trạng thái: " + book.status
            bookDetailViewModel.categoryNameString  = "Thể loại: " + book.categoryName
            bookDetailViewModel.likeString = book.like.toString()
            bookDetailViewModel.viewString = book.view.toString()
            bookDetailViewModel.chapterTotalString = book.chapterTotal.toString()
            binding.viewModel = bookDetailViewModel
            binding.executePendingBindings()
            //binding.textViewChapterTotal.text = Html.fromHtml(result?.intro, Html.FROM_HTML_MODE_COMPACT)
            binding.bookDetailIntro.text = Html.fromHtml(book.intro, Html.FROM_HTML_MODE_COMPACT)

            Glide.with(binding.root)
                .load(result.cover)
                .apply(RequestOptions.bitmapTransform(BlurTransformation(25, 5)))
                .into(binding.bookDetailBackground)

            Glide.with(binding.root)
                .load(result.cover)
                .placeholder(R.drawable.qd_book_cover)
                .into(binding.toolbarContentImage)

            Glide.with(binding.root)
                .load(result.cover)
                .placeholder(R.drawable.qd_book_cover)
                .into(binding.bookDetailCoverImage)

            val query: String = "where:authorName=" + book.authorName + ";where:name!" + book.name
            getSameAuthorJob = initialBook(binding.recyclerSameAuthor, sameAuthorAdapter, getSameAuthorJob, query)

            binding.setClickChapterListListener { view ->
                val direction =
                    BookDetailFragmentDirections.actionBookDetailFragmentToChapterListFragment(book.id)
                view.findNavController().navigate(direction)
            }

            binding.setClickFirstChapterListener { view ->
                val direction =
                    BookDetailFragmentDirections.actionBookDetailFragmentToChapterContentFragment(bookId = book.id, order = 1, chapterId = "")
                view.findNavController().navigate(direction)
            }
        }

        val chapterRepository = ChapterRepository(ChapterService.create())
        chapterRepository.getLastChapter(bookDetailViewModel.bookId) { chapter: DataChapter ->
            binding.lastChapterNameBookDetail.text = chapter.name
        }

        binding.lifecycleOwner = viewLifecycleOwner

        binding.bookDetailAppBar.addOnOffsetChangedListener(object : AppBarStateChangeListener() {
            override fun onStateChanged(appBarLayout: AppBarLayout?, state: State?) {
                when(state) {
                    State.COLLAPSED -> binding.bookDetailToolbarContent.visibility = VISIBLE
                    State.EXPANDED -> binding.bookDetailToolbarContent.visibility = INVISIBLE
                    State.IDLE -> {
                        if(binding.bookDetailToolbarContent.visibility == VISIBLE){
                            binding.bookDetailToolbarContent.visibility = INVISIBLE
                        }
                    }
                }
            }
        })




        return binding.root
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
            bookDetailViewModel.getBooks(query).collectLatest {
                bookAdapter.submitData(it)
            }
        }
        return newJob

    }

    abstract class AppBarStateChangeListener : OnOffsetChangedListener {
        enum class State {
            EXPANDED, COLLAPSED, IDLE
        }

        private var mCurrentState = State.IDLE
        override fun onOffsetChanged(appBarLayout: AppBarLayout, i: Int) {
            mCurrentState = when {
                i == 0 -> {
                    if (mCurrentState != State.EXPANDED) {
                        onStateChanged(appBarLayout, State.EXPANDED)
                    }
                    State.EXPANDED
                }
                abs(i) >= appBarLayout.totalScrollRange -> {
                    if (mCurrentState != State.COLLAPSED) {
                        onStateChanged(appBarLayout, State.COLLAPSED)
                    }
                    State.COLLAPSED
                }
                else -> {
                    if (mCurrentState != State.IDLE) {
                        onStateChanged(appBarLayout, State.IDLE)
                    }
                    State.IDLE
                }
            }
        }

        abstract fun onStateChanged(appBarLayout: AppBarLayout?, state: State?)
    }


}