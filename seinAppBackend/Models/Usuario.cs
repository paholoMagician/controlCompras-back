﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace seinAppBackend.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Password { get; set; }

    public int? Edad { get; set; }

    public string Email { get; set; }

    public int? Tipo { get; set; }

    public int? Estado { get; set; }

    public string Direccion { get; set; }

    public string Cedula { get; set; }

    public DateTime? Fecrea { get; set; }

    public int? Usercrea { get; set; }

    public string Telefono { get; set; }
}