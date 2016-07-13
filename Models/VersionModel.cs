using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class VersionModel
    {
        [Key]
        [Column(Order = 1)]
        [DataMember(Name = "ComputerName")]
        public string ComputerName { get; set; }
        [Key]
        [Column(Order = 2)]
        [DataMember(Name = "ComputerGroup")]
        public string ComputerGroup { get; set; }
        [Key]
        [Column(Order = 3)]
        [DataMember(Name = "ApplicationName")]
        public string ApplicationName { get; set; }
        [DataMember(Name = "ApplicationVersion")]
        public string ApplicationVersion { get; set; }
        public DateTime LastCheckTimestamp { get; set; }
    }
}