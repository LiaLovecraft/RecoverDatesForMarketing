# RecoverDatesForMarketing
Coding exercise for Valant

A program to attempt to recover dates from a possibly corrupted data file. 

'''
Usage: RecoverDatesForMarketing <file_to_parse>
'''

Actually, the very first thing I would do would be to see if there is any possible way to get a fresh export - corrupted data 
files are of course not reliable, and I would personally hate for decisions of any sort to be made based on them. Failing this, 
I would see if there are any examples of previously received non-corrupt files from the same source. Having said that, I did go 
ahead and code a first version of a recovery attempt.

I chose to do this via the following process:

1. Strip out non-printing characters using a regular expression
2. Identify "date-like" 8 digit strings using another regular expression. This results in the first two characters being integers between 01 and 12, the next two characters being integers between 01 and 31, and the last four characters being integers
3. Use DateTime.TryParseExact to filter out non-dates from step 2 (i.e., leap year violations, "June 31st" dates, etc.)
4. Filter using today's date to exclude any dates from step 3 >= today

The reasons I chose to do it in this way are that first, using regex to strip possible corrupt non-printing characters and then 
again using regex to identify date-like strings is a quick and easy way to start; second, DateTime.TryParseExact will ensure 
that any date-like but incorrect values are filtered out; third, this is a quick first shot at things which I can then review 
and iteratively alter / improve with my business users' input. It lets me get some data in front of them quickly, and their input 
can then help determine whether these dates look good and perhaps the corruption was minor, or if the problem is a more major one.


I have concerns and questions about this, which I would convey to marketing: 

1. Even the exact-match dates pulled from the file may be false positives, as the corruption may have produced a valid date due to character substitution or insertion. 
2. I would ask if the dates I was able to recover match their expectations considering the intent of the data (i.e., dates in the middle ages and before might be expected if the subject matter was "History of Feudalism in Europe," but not if the subject matter was "History of the Chevrolet Corvette").
3. I would show them the exact date matches, and then explain that I could take further and ever-less-confident steps to try to pull dates out, such as:
  1. checking each 8-digit number that doesn't evaluate to a valid date to see if mutations (substitutions, transposition) would render possibly valid dates
  2. checking to see if there are possible insertions in the middle of dates, such that removing a set of numerals reveals a valid date possibility
4. I would stress to them that #3 would result in a high probability of false positives. If they did want to see the results of this, I would devise some arbitrary confidence scale, and reduce confidence with each manipulation required to produce a possible date match.
