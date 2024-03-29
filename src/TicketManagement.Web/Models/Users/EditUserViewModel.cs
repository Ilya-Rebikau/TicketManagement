﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketManagement.Web.Models.Users
{
    /// <summary>
    /// Edit user view model.
    /// </summary>
    public class EditUserViewModel
    {
        /// <summary>
        /// Get or sets user id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets user email.
        /// </summary>
        [Display(Name="Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets surname.
        /// </summary>
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets balance.
        /// </summary>
        [Display(Name = "Balance")]
        [Range(0, double.MaxValue)]
        public double Balance { get; set; }

        /// <summary>
        /// Gets or sets time zone.
        /// </summary>
        [Display(Name = "TimeZone")]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets all time zones.
        /// </summary>
        public SelectList TimeZones { get; } = new SelectList(GetStandartTimeZones());

        /// <summary>
        /// Get list with all standart time zones.
        /// </summary>
        /// <returns>List with time zones.</returns>
        private static List<string> GetStandartTimeZones()
        {
            var stringTimeZones = new List<string>();
            var list = TimeZoneInfo.GetSystemTimeZones();
            foreach (var item in list)
            {
                stringTimeZones.Add(item.Id);
            }

            return stringTimeZones;
        }
    }
}
