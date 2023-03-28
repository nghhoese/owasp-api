# Prevent malformed data

## Input Validation

### Text

### File

### Special attention
Input Validation should not be used as the primary method of preventing XSS, SQL Injection but can significantly contribute to reducing their impact if implemented properly.

.NET Encoding Examples
Starting with .NET 4.5 , the Anti-Cross Site Scripting library is part of the framework, but not enabled by default. You can specify to use the AntiXssEncoder from this library as the default encoder for your entire application using the web.conf settings.
For more more information about encoding in different programming languages, read OWASP's Proactive Controls about: [Encode and Escape Data](https://owasp.org/www-project-proactive-controls/v3/en/c4-encode-escape-data)