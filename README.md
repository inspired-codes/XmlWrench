# XmlWrench
This XML tool helps to sanitize data block in between *valid* tags.

## Example
sanitizing data block between &lt;INFO&gt; tags  
```
<INFO language="en">Nice & Big Info, the cat size < dog size</INFO>
```
results in
```xml
<INFO language="en">Nice &amp; Big Info, the cat size &lt; dog size</INFO>
```

- The ```<``` must be escaped with a ```&lt;``` - assumed to be the beginning of a tag
- The ```>``` should be escaped with ```&gt;``` - not mandatory, depends on context, strongly advised to escape it
- The ```&``` must be escaped with a ```&amp;``` - assumed to be the beginning a entity reference


## Distinciton
XmlDataWrench does not sanitize invalid chars inside a TAG

## References
[XML charsets](https://www.w3.org/TR/xml/#charsets)

## "Turing Machine"  
Method call cycle, showing loops, end and exception end states  
- main loop
  ![XML_Wrench_Main_Loop](Documentation/XML%20Data%20Wrench.bpmn.png)  
- tag loop
  ![XML Wrench Tag Parser Loop](Documentation/XML%20Data%20Wrench-Tag%20and%20Rest%20Parser.png)  
- data block loop
  ![XML Wrench Data Parser Loop](Documentation/XML%20Data%20Wrench-Data%20block%20Parser.png)  


## Thanks to ...
[Stackoverflow @potame](https://stackoverflow.com/questions/730133/what-are-invalid-characters-in-xml)

## Next ...
### XmlWrench features in my mind
- optimize peak ahead method that checks if two '&lt;' chars are in sequence
- sanitize TAG based on list of valid tag names
-> sanitize attribute values
-> sanitize attribute names


### You can contribute 
... by creating your own branch and sending me a pull request.  
Please do not be dissapointed if my response time varies.  

