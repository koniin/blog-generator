## Synopsis

A simple static blog generator written in C# &amp;&amp; dotnetcore.
Blog generator generates html from a set of markdown files that contain your information, typically blog posts. This is a very simple generator with only a few options of how the outcome will be. It has one template file that it bases all out put on. 

## How

A template file is used to generate the blog, a sample can be found in the template folder.
Settings for generation and folder structure can be edited in the settings.config file.
You can specify additional directories to be copied from your template directory (img/js/css etc) to your output directory (default: html). Set these in settings.config CopyDirectories parameter, delimited by "," character.

#### Parameters
Add a post:
blog-generator -p [Your post title]

Edit the post text in src directory by replacing the {{text}} with your text

Generate blog:
blog-generator -g

Generates the complete blog into the output directory

## Motivation

I just needed a simple easy tool to generate a blog and this fits all my needs. It can generate a blog and that's it. 

## Usage

Download the source or clone it

Either run it directly with
"dotnet run -- -p [YOURPOSTNAME]"
"dotnet run -- -g"

or

"dotnet build" then copy the output from bin/Debug/netcoreapp2.0/blog-generator.dll to a directory of your liking and run with "dotnet blog-generator.dll [ARGUMENTS]"

or

You can edit the config and create an exe file or perhaps contact me to make one for you, if you are nice ;)

## Testing

There are a lot of tests for this app that are not yet written ;)

## Contributors

@henkearnssn

## License

Do whatever you want, GL HF!