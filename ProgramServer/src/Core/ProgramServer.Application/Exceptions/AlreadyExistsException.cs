﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramServer.Application.Exceptions
{
    public class AlreadyExistsException : ApplicationException
    {
        public AlreadyExistsException(string name, object key)
            : base($"Entity {name} - ({key}) მომხმარებელი ასეთი სახელით უკვე არსებობს.")
        {
        }
    }
}
