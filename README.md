Bigram
=============

A command line tool to parse a text and output a histogram of the bigrams contained in it.

* can receive the text via file or command line
* utilizes VAE.CLI.Flags for command line options parsing
* Bigrams parsing ends at sentence boundaries by default but can cross by use the -xsb option

Examples
-------
	Bigram.exe --filepath LoremIpsum.txt

	Bigram.exe The Quick Brown fox jumped over the quick blue hare.

License
=======

Licensed under the http://www.apache.org/licenses/LICENSE-2.0 License.
