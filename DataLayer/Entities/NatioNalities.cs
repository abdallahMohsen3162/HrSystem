﻿namespace DataLayer.Entities

{

    public class ListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class Nationalities
    {

        public static List<ListItem> GetNationalities()
        {
            List<ListItem> options = new List<ListItem>
            {

                new ListItem { Text = "Afghanistan", Value ="AF" },
                new ListItem { Text = "Aland Islands", Value ="AX" },
                new ListItem { Text = "Albania", Value ="AL" },
                new ListItem { Text = "Algeria", Value ="DZ" },
                new ListItem { Text = "American Samoa", Value ="AS" },
                new ListItem { Text = "Andorra", Value ="AD" },
                new ListItem { Text = "Angola", Value ="AO" },
                new ListItem { Text = "Anguilla", Value ="AI" },
                new ListItem { Text = "Antigua & Barbuda", Value ="AG" },
                new ListItem { Text = "Argentina", Value ="AR" },
                new ListItem { Text = "Armenia", Value ="AM" },
                new ListItem { Text = "Aruba", Value ="AW" },
                new ListItem { Text = "Australia", Value ="AU" },
                new ListItem { Text = "Austria", Value ="AT" },
                new ListItem { Text = "Azerbaijan", Value ="AZ" },
                new ListItem { Text = "Bahamas", Value ="BS" },
                new ListItem { Text = "Bahrain", Value ="BH" },
                new ListItem { Text = "Bangladesh", Value ="BD" },
                new ListItem { Text = "Barbados", Value ="BB" },
                new ListItem { Text = "Belarus", Value ="BY" },
                new ListItem { Text = "Belgium", Value ="BE" },
                new ListItem { Text = "Belize", Value ="BZ" },
                new ListItem { Text = "Benin", Value ="BJ" },
                new ListItem { Text = "Bermuda", Value ="BM" },
                new ListItem { Text = "Bhutan", Value ="BT" },
                new ListItem { Text = "Bolivia", Value ="BO" },
                new ListItem { Text = "Bosnia and Herzegovina", Value ="BA" },
                new ListItem { Text = "Botswana", Value ="BW" },
                new ListItem { Text = "Bouvet Island", Value ="BV" },
                new ListItem { Text = "Brazil", Value ="BR" },
                new ListItem { Text = "British Indian Ocean Territory", Value ="IO" },
                new ListItem { Text = "Brunei", Value ="BN" },
                new ListItem { Text = "Bulgaria", Value ="BG" },
                new ListItem { Text = "Burkina Faso", Value ="BF" },
                new ListItem { Text = "Burundi", Value ="BI" },
                new ListItem { Text = "Cambodia", Value ="KH" },
                new ListItem { Text = "Cameroon", Value ="CM" },
                new ListItem { Text = "Canada", Value ="CA" },
                new ListItem { Text = "Cape Verde", Value ="CV" },
                new ListItem { Text = "Caribbean Netherlands", Value ="BQ" },
                new ListItem { Text = "Cayman Islands", Value ="KY" },
                new ListItem { Text = "Central African Republic", Value ="CF" },
                new ListItem { Text = "Chad", Value ="TD" },
                new ListItem { Text = "Chile", Value ="CL" },
                new ListItem { Text = "China", Value ="CN" },
                new ListItem { Text = "Christmas Island", Value ="CX" },
                new ListItem { Text = "Cocos (Keeling) Islands", Value ="CC" },
                new ListItem { Text = "Colombia", Value ="CO" },
                new ListItem { Text = "Comoros", Value ="KM" },
                new ListItem { Text = "Congo, Democratic Republic Of The", Value ="CD" },
                new ListItem { Text = "Congo, Republic of the", Value ="CG" },
                new ListItem { Text = "Cook Islands", Value ="CK" },
                new ListItem { Text = "Costa Rica", Value ="CR" },
                new ListItem { Text = "Cote d'Ivoire (Ivory Coast)", Value ="CI" },
                new ListItem { Text = "Croatia", Value ="HR" },
                new ListItem { Text = "Cuba", Value ="CU" },
                new ListItem { Text = "Curacao", Value ="CW" },
                new ListItem { Text = "Cyprus", Value ="CY" },
                new ListItem { Text = "Czech Republic", Value ="CZ" },
                new ListItem { Text = "Denmark", Value ="DK" },
                new ListItem { Text = "Djibouti", Value ="DJ" },
                new ListItem { Text = "Dominica", Value ="DM" },
                new ListItem { Text = "Dominican Republic", Value ="DO" },
                new ListItem { Text = "Ecuador", Value ="EC" },
                new ListItem { Text = "Egypt", Value ="EG" },
                new ListItem { Text = "El Salvador", Value ="SV" },
                new ListItem { Text = "Equatorial Guinea", Value ="GQ" },
                new ListItem { Text = "Eritrea", Value ="ER" },
                new ListItem { Text = "Estonia", Value ="EE" },
                new ListItem { Text = "Ethiopia", Value ="ET" },
                new ListItem { Text = "Falkland Islands", Value ="FK" },
                new ListItem { Text = "Faroe Islands", Value ="FO" },
                new ListItem { Text = "Fiji", Value ="FJ" },
                new ListItem { Text = "Finland", Value ="FI" },
                new ListItem { Text = "France", Value ="FR" },
                new ListItem { Text = "French Guiana", Value ="GF" },
                new ListItem { Text = "French Polynesia", Value ="PF" },
                new ListItem { Text = "French Southern Territories", Value ="TF" },
                new ListItem { Text = "Gabon", Value ="GA" },
                new ListItem { Text = "Gambia", Value ="GM" },
                new ListItem { Text = "Georgia", Value ="GE" },
                new ListItem { Text = "Germany", Value ="DE" },
                new ListItem { Text = "Ghana", Value ="GH" },
                new ListItem { Text = "Gibraltar", Value ="GI" },
                new ListItem { Text = "Greece", Value ="GR" },
                new ListItem { Text = "Greenland", Value ="GL" },
                new ListItem { Text = "Grenada", Value ="GD" },
                new ListItem { Text = "Guadeloupe", Value ="GP" },
                new ListItem { Text = "Guam", Value ="GU" },
                new ListItem { Text = "Guatemala", Value ="GT" },
                new ListItem { Text = "Guernsey", Value ="GG" },
                new ListItem { Text = "Guinea", Value ="GN" },
                new ListItem { Text = "Guinea Bissau", Value ="GW" },
                new ListItem { Text = "Guyana", Value ="GY" },
                new ListItem { Text = "Haiti", Value ="HT" },
                new ListItem { Text = "Heard and McDonald Islands", Value ="HM" },
                new ListItem { Text = "Honduras", Value ="HN" },
                new ListItem { Text = "Hong Kong", Value ="HK" },
                new ListItem { Text = "Hungary", Value ="HU" },
                new ListItem { Text = "Iceland", Value ="IS" },
                new ListItem { Text = "India", Value ="IN" },
                new ListItem { Text = "Indonesia", Value ="ID" },
                new ListItem { Text = "Iran", Value ="IR" },
                new ListItem { Text = "Iraq", Value ="IQ" },
                new ListItem { Text = "Ireland, Republic of", Value ="IE" },
                new ListItem { Text = "Isle of Man", Value ="IM" },
                new ListItem { Text = "Israel", Value ="IL" },
                new ListItem { Text = "Italy", Value ="IT" },
                new ListItem { Text = "Jamaica", Value ="JM" },
                new ListItem { Text = "Japan", Value ="JP" },
                new ListItem { Text = "Jersey", Value ="JE" },
                new ListItem { Text = "Jordan", Value ="JO" },
                new ListItem { Text = "Kazakhstan", Value ="KZ" },
                new ListItem { Text = "Kenya", Value ="KE" },
                new ListItem { Text = "Kiribati", Value ="KI" },
                new ListItem { Text = "Korea, Democratic People's Republic of", Value ="KP" },
                new ListItem { Text = "Korea, Republic Of", Value ="KR" },
                new ListItem { Text = "Kosovo", Value ="XK" },
                new ListItem { Text = "Kuwait", Value ="KW" },
                new ListItem { Text = "Kyrgyzstan", Value ="KG" },
                new ListItem { Text = "Laos", Value ="LA" },
                new ListItem { Text = "Latvia", Value ="LV" },
                new ListItem { Text = "Lebanon", Value ="LB" },
                new ListItem { Text = "Lesotho", Value ="LS" },
                new ListItem { Text = "Liberia", Value ="LR" },
                new ListItem { Text = "Libya", Value ="LY" },
                new ListItem { Text = "Liechtenstein", Value ="LI" },
                new ListItem { Text = "Lithuania", Value ="LT" },
                new ListItem { Text = "Luxembourg", Value ="LU" },
                new ListItem { Text = "Macau", Value ="MO" },
                new ListItem { Text = "Macedonia", Value ="MK" },
                new ListItem { Text = "Madagascar", Value ="MG" },
                new ListItem { Text = "Malawi", Value ="MW" },
                new ListItem { Text = "Malaysia", Value ="MY" },
                new ListItem { Text = "Maldives", Value ="MV" },
                new ListItem { Text = "Mali", Value ="ML" },
                new ListItem { Text = "Malta", Value ="MT" },
                new ListItem { Text = "Marshall Islands", Value ="MH" },
                new ListItem { Text = "Martinique", Value ="MQ" },
                new ListItem { Text = "Mauritania", Value ="MR" },
                new ListItem { Text = "Mauritius", Value ="MU" },
                new ListItem { Text = "Mayotte", Value ="YT" },
                new ListItem { Text = "Mexico", Value ="MX" },
                new ListItem { Text = "Micronesia", Value ="FM" },
                new ListItem { Text = "Moldova", Value ="MD" },
                new ListItem { Text = "Monaco", Value ="MC" },
                new ListItem { Text = "Mongolia", Value ="MN" },
                new ListItem { Text = "Montenegro", Value ="ME" },
                new ListItem { Text = "Montserrat", Value ="MS" },
                new ListItem { Text = "Morocco", Value ="MA" },
                new ListItem { Text = "Mozambique", Value ="MZ" },
                new ListItem { Text = "Myanmar", Value ="MM" },
                new ListItem { Text = "Namibia", Value ="NA" },
                new ListItem { Text = "Nauru", Value ="NR" },
                new ListItem { Text = "Nepal", Value ="NP" },
                new ListItem { Text = "Netherlands", Value ="NL" },
                new ListItem { Text = "Netherlands Antilles", Value ="AN" },
                new ListItem { Text = "New Caledonia", Value ="NC" },
                new ListItem { Text = "New Zealand", Value ="NZ" },
                new ListItem { Text = "Nicaragua", Value ="NI" },
                new ListItem { Text = "Niger", Value ="NE" },
                new ListItem { Text = "Nigeria", Value ="NG" },
                new ListItem { Text = "Niue", Value ="NU" },
                new ListItem { Text = "Norfolk Island", Value ="NF" },
                new ListItem { Text = "Northern Mariana Islands", Value ="MP" },
                new ListItem { Text = "Norway", Value ="NO" },
                new ListItem { Text = "Oman", Value ="OM" },
                new ListItem { Text = "Pakistan", Value ="PK" },
                new ListItem { Text = "Palau", Value ="PW" },
                new ListItem { Text = "Palestine", Value ="PS" },
                new ListItem { Text = "Panama", Value ="PA" },
                new ListItem { Text = "Papua New Guinea", Value ="PG" },
                new ListItem { Text = "Paraguay", Value ="PY" },
                new ListItem { Text = "Peru", Value ="PE" },
                new ListItem { Text = "Philippines", Value ="PH" },
                new ListItem { Text = "Pitcairn Islands", Value ="PN" },
                new ListItem { Text = "Poland", Value ="PL" },
                new ListItem { Text = "Portugal", Value ="PT" },
                new ListItem { Text = "Puerto Rico", Value ="PR" },
                new ListItem { Text = "Qatar", Value ="QA" },
                new ListItem { Text = "Reunion", Value ="RE" },
                new ListItem { Text = "Romania", Value ="RO" },
                new ListItem { Text = "Russian Federation", Value ="RU" },
                new ListItem { Text = "Rwanda", Value ="RW" },
                new ListItem { Text = "Saint Barthelemy", Value ="BL" },
                new ListItem { Text = "Saint Helena", Value ="SH" },
                new ListItem { Text = "Saint Kitts & Nevis", Value ="KN" },
                new ListItem { Text = "Saint Lucia", Value ="LC" },
                new ListItem { Text = "Saint Martin (France)", Value ="MF" },
                new ListItem { Text = "Saint Martin (Netherlands)", Value ="SX" },
                new ListItem { Text = "Saint Pierre & Miquelon", Value ="PM" },
                new ListItem { Text = "Saint Vincent and Grenadines", Value ="VC" },
                new ListItem { Text = "Samoa", Value ="WS" },
                new ListItem { Text = "San Marino", Value ="SM" },
                new ListItem { Text = "Sao Tome and Príncipe", Value ="ST" },
                new ListItem { Text = "Saudi Arabia", Value ="SA" },
                new ListItem { Text = "Senegal", Value ="SN" },
                new ListItem { Text = "Serbia", Value ="RS" },
                new ListItem { Text = "Seychelles", Value ="SC" },
                new ListItem { Text = "Sierra Leone", Value ="SL" },
                new ListItem { Text = "Singapore", Value ="SG" },
                new ListItem { Text = "Slovakia", Value ="SK" },
                new ListItem { Text = "Slovenia", Value ="SI" },
                new ListItem { Text = "Solomon Islands", Value ="SB" },
                new ListItem { Text = "Somalia", Value ="SO" },
                new ListItem { Text = "South Africa", Value ="ZA" },
                new ListItem { Text = "South Georgia and the South Sandwich Islands", Value ="GS" },
                new ListItem { Text = "South Sudan", Value ="SS" },
                new ListItem { Text = "Spain", Value ="ES" },
                new ListItem { Text = "Sri Lanka", Value ="LK" },
                new ListItem { Text = "Sudan", Value ="SD" },
                new ListItem { Text = "Suriname", Value ="SR" },
                new ListItem { Text = "Svalbard and Jan Mayen Islands", Value ="SJ" },
                new ListItem { Text = "Swaziland", Value ="SZ" },
                new ListItem { Text = "Sweden", Value ="SE" },
                new ListItem { Text = "Switzerland", Value ="CH" },
                new ListItem { Text = "Syria", Value ="SY" },
                new ListItem { Text = "Taiwan, China", Value ="TW" },
                new ListItem { Text = "Tajikistan", Value ="TJ" },
                new ListItem { Text = "Tanzania", Value ="TZ" },
                new ListItem { Text = "Thailand", Value ="TH" },
                new ListItem { Text = "Timor Leste", Value ="TL" },
                new ListItem { Text = "Togo", Value ="TG" },
                new ListItem { Text = "Tokelau", Value ="TK" },
                new ListItem { Text = "Tonga", Value ="TO" },
                new ListItem { Text = "Trinidad & Tobago", Value ="TT" },
                new ListItem { Text = "Tunisia", Value ="TN" },
                new ListItem { Text = "Turkey", Value ="TR" },
                new ListItem { Text = "Turkmenistan", Value ="TM" },
                new ListItem { Text = "Turks and Caicos Islands", Value ="TC" },
                new ListItem { Text = "Tuvalu", Value ="TV" },
                new ListItem { Text = "Uganda", Value ="UG" },
                new ListItem { Text = "Ukraine", Value ="UA" },
                new ListItem { Text = "United Arab Emirates", Value ="AE" },
                new ListItem { Text = "United Kingdom", Value ="GB" },
                new ListItem { Text = "United States Minor Outlying Islands", Value ="UM" },
                new ListItem { Text = "United States of America", Value ="US" },
                new ListItem { Text = "Uruguay", Value ="UY" },
                new ListItem { Text = "Uzbekistan", Value ="UZ" },
                new ListItem { Text = "Vanuatu", Value ="VU" },
                new ListItem { Text = "Vatican City", Value ="VA" },
                new ListItem { Text = "Venezuela", Value ="VE" },
                new ListItem { Text = "Vietnam", Value ="VN" },
                new ListItem { Text = "Virgin Islands (British)", Value ="VG" },
                new ListItem { Text = "Virgin Islands (U.S.)", Value ="VI" },
                new ListItem { Text = "Wallis & Futuna", Value ="WF" },
                new ListItem { Text = "Yemen", Value ="YE" },
                new ListItem { Text = "Zambia", Value ="ZM" },
                new ListItem { Text = "Zimbabwe", Value ="ZW" }
            };

            return options.OrderBy(c => c.Text).ToList();
        }

    }
}