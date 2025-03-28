﻿namespace Phemedrone.Classes;

public class Cookie
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Domain { get; set; }
    public string Expires { get; set; }
    public string HttpOnly { get; set; }
    public string Secure { get; set; }
    public string Value { get; set; }
}