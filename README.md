# SudokuSolver
This is a demo for applying the Dancing Links algorithm to solving Sudoku grids


#NOTE TO SELF:

http://lanl.arxiv.org/pdf/cs/0011047
http://garethrees.org/2007/06/10/zendoku-generation/#section-4

You are in the middle of writing the backtracking. Previously you were storing rows but I think it needs to be either columns or nodes. You had just thrown most of your custom stuff out and decided to just
straight up code what Knuth had. Also he suggests having a pointer back to the top (non-header) element so you may need to make that as well.