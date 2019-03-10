﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Anotar.NLog;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class InProgressSubjectApproach
    {
        public string SemesterLiteral { get; set; }

        public string Id { get; set; }

        public string Acronym => Id.Substring(startIndex: Id.LastIndexOf('-') + 1);
    }
}
 