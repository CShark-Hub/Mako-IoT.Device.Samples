import { h, FunctionComponent } from "preact";

interface CalendarSectionProps {
  url: string;
  timeZone: string;
  onURLChange: (value: string) => void;
  onTimeZoneChange: (value: string) => void;
}

const CalendarSection: FunctionComponent<CalendarSectionProps> = ({
  url,
  timeZone,
  onURLChange,
  onTimeZoneChange,
}) => {
  return (
    <div className="mb-3">
      <h3 className="mb-3">Calendar Settings</h3>

      <div className="mb-3">
        <label htmlFor="url" className="form-label">
          URL:
        </label>

        <input
          type="text"
          id="url"
          className="form-control"
          value={url}
          onChange={(e) => onURLChange(e.currentTarget.value)}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="timeZone">Time Zone:</label>
        <select
          id="timeZone"
          className="form-select"
          value={timeZone}
          onChange={(e) => onTimeZoneChange(e.currentTarget.value)}
        >
          <option value="<-12>12;0">
            (UTC-12:00) International Date Line West
          </option>
          <option value="<-11>11;1">
            (UTC-11:00) Coordinated Universal Time-11
          </option>
          <option value="HST10HDT,M3.2.0,M11.1.0;2">
            (UTC-10:00) Aleutian Islands
          </option>
          <option value="HST10;3">(UTC-10:00) Hawaii</option>
          <option value="<-0930>9:30;4">(UTC-09:30) Marquesas Islands</option>
          <option value="AKST9AKDT,M3.2.0,M11.1.0;5">(UTC-09:00) Alaska</option>
          <option value="<-09>9;6">
            (UTC-09:00) Coordinated Universal Time-09
          </option>
          <option value="PST8PDT,M3.2.0,M11.1.0;7">
            (UTC-08:00) Baja California
          </option>
          <option value="<-08>8;8">
            (UTC-08:00) Coordinated Universal Time-08
          </option>
          <option value="PST8PDT,M3.2.0,M11.1.0;9">
            (UTC-08:00) Pacific Time (US & Canada)
          </option>
          <option value="MST7;10">(UTC-07:00) Arizona</option>
          <option value="MST7;11">(UTC-07:00) La Paz, Mazatlan</option>
          <option value="MST7MDT,M3.2.0,M11.1.0;12">
            (UTC-07:00) Mountain Time (US & Canada)
          </option>
          <option value="MST7;13">(UTC-07:00) Yukon</option>
          <option value="CST6;14">(UTC-06:00) Central America</option>
          <option value="CST6CDT,M3.2.0,M11.1.0;15">
            (UTC-06:00) Central Time (US & Canada)
          </option>
          <option value="<-06>6<-05>,M9.1.6/22,M4.1.6/22;16">
            (UTC-06:00) Easter Island
          </option>
          <option value="CST6;17">
            (UTC-06:00) Guadalajara, Mexico City, Monterrey
          </option>
          <option value="CST6;18">(UTC-06:00) Saskatchewan</option>
          <option value="<-05>5;19">
            (UTC-05:00) Bogota, Lima, Quito, Rio Branco
          </option>
          <option value="EST5;20">(UTC-05:00) Chetumal</option>
          <option value="EST5EDT,M3.2.0,M11.1.0;21">
            (UTC-05:00) Eastern Time (US & Canada)
          </option>
          <option value="EST5EDT,M3.2.0,M11.1.0;22">(UTC-05:00) Haiti</option>
          <option value="CST5CDT,M3.2.0/0,M11.1.0/1;23">
            (UTC-05:00) Havana
          </option>
          <option value="EST5EDT,M3.2.0,M11.1.0;24">
            (UTC-05:00) Indiana (East)
          </option>
          <option value="EST5EDT,M3.2.0,M11.1.0;25">
            (UTC-05:00) Turks and Caicos
          </option>
          <option value="<-04>4<-03>,M10.1.0/0,M3.4.0/0;26">
            (UTC-04:00) Asuncion
          </option>
          <option value="AST4ADT,M3.2.0,M11.1.0;27">
            (UTC-04:00) Atlantic Time (Canada)
          </option>
          <option value="<-04>4;28">(UTC-04:00) Caracas</option>
          <option value="<-04>4;29">(UTC-04:00) Cuiaba</option>
          <option value="<-04>4;30">
            (UTC-04:00) Georgetown, La Paz, Manaus, San Juan
          </option>
          <option value="<-04>4<-03>,M9.2.0/0,M4.1.0/0;31">
            (UTC-04:00) Santiago
          </option>
          <option value="NST3:30NDT,M3.2.0,M11.1.0;32">
            (UTC-03:30) Newfoundland
          </option>
          <option value="<-03>3;33">(UTC-03:00) Araguaina</option>
          <option value="<-03>3;34">(UTC-03:00) Brasilia</option>
          <option value="<-03>3;35">(UTC-03:00) Cayenne, Fortaleza</option>
          <option value="<-03>3;36">(UTC-03:00) City of Buenos Aires</option>
          <option value="<-03>3;37">(UTC-03:00) Montevideo</option>
          <option value="<-03>3;38">(UTC-03:00) Punta Arenas</option>
          <option value="<-03>3<-02>,M3.2.0,M11.1.0;39">
            (UTC-03:00) Saint Pierre and Miquelon
          </option>
          <option value="<-03>3;40">(UTC-03:00) Salvador</option>
          <option value="<-02>2;41">
            (UTC-02:00) Coordinated Universal Time-02
          </option>
          <option value="<-02>2;42">(UTC-02:00) Greenland</option>
          <option value="<-02>2;43">(UTC-02:00) Mid-Atlantic - Old</option>
          <option value="<-01>1<+00>,M3.5.0/0,M10.5.0/1;44">
            (UTC-01:00) Azores
          </option>
          <option value="<-01>1;45">(UTC-01:00) Cabo Verde Is.</option>
          <option value="UTC0;46">(UTC) Coordinated Universal Time</option>
          <option value="GMT0BST,M3.5.0/1,M10.5.0;47">
            (UTC+00:00) Dublin, Edinburgh, Lisbon, London
          </option>
          <option value="GMT0;48">(UTC+00:00) Monrovia, Reykjavik</option>
          <option value="GMT0;49">(UTC+00:00) Sao Tome</option>
          <option value="<+01>-1;50">(UTC+01:00) Casablanca</option>
          <option value="CET-1CEST,M3.5.0,M10.5.0/3;51">
            (UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna
          </option>
          <option value="CET-1CEST,M3.5.0,M10.5.0/3;52">
            (UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague
          </option>
          <option value="CET-1CEST,M3.5.0,M10.5.0/3;53">
            (UTC+01:00) Brussels, Copenhagen, Madrid, Paris
          </option>
          <option value="CET-1CEST,M3.5.0,M10.5.0/3;54">
            (UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb
          </option>
          <option value="WAT-1;55">(UTC+01:00) West Central Africa</option>
          <option value="EET-2EEST,M3.5.0/3,M10.5.0/4;56">
            (UTC+02:00) Athens, Bucharest
          </option>
          <option value="EET-2EEST,M3.5.0/0,M10.5.0/0;57">
            (UTC+02:00) Beirut
          </option>
          <option value="EET-2;58">(UTC+02:00) Cairo</option>
          <option value="EET-2EEST,M3.5.0,M10.5.0/3;59">
            (UTC+02:00) Chisinau
          </option>
          <option value="EET-2EEST,M3.5.6,M10.5.6;60">
            (UTC+02:00) Gaza, Hebron
          </option>
          <option value="SAST-2;61">(UTC+02:00) Harare, Pretoria</option>
          <option value="EET-2EEST,M3.5.0/3,M10.5.0/4;62">
            (UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius
          </option>
          <option value="IST-2IDT,M3.5.5,M10.5.0;63">
            (UTC+02:00) Jerusalem
          </option>
          <option value="CAT-2;64">(UTC+02:00) Juba</option>
          <option value="EET-2;65">(UTC+02:00) Kaliningrad</option>
          <option value="CAT-2;66">(UTC+02:00) Khartoum</option>
          <option value="EET-2;67">(UTC+02:00) Tripoli</option>
          <option value="CAT-2;68">(UTC+02:00) Windhoek</option>
          <option value="<+03>-3;69">(UTC+03:00) Amman</option>
          <option value="<+03>-3;70">(UTC+03:00) Baghdad</option>
          <option value="<+03>-3;71">(UTC+03:00) Damascus</option>
          <option value="<+03>-3;72">(UTC+03:00) Istanbul</option>
          <option value="<+03>-3;73">(UTC+03:00) Kuwait, Riyadh</option>
          <option value="<+03>-3;74">(UTC+03:00) Minsk</option>
          <option value="MSK-3;75">(UTC+03:00) Moscow, St. Petersburg</option>
          <option value="EAT-3;76">(UTC+03:00) Nairobi</option>
          <option value="<+03>-3;77">(UTC+03:00) Volgograd</option>
          <option value="<+0330>-3:30;78">(UTC+03:30) Tehran</option>
          <option value="<+04>-4;79">(UTC+04:00) Abu Dhabi, Muscat</option>
          <option value="<+04>-4;80">(UTC+04:00) Astrakhan, Ulyanovsk</option>
          <option value="<+04>-4;81">(UTC+04:00) Baku</option>
          <option value="<+04>-4;82">(UTC+04:00) Izhevsk, Samara</option>
          <option value="<+04>-4;83">(UTC+04:00) Port Louis</option>
          <option value="<+04>-4;84">(UTC+04:00) Saratov</option>
          <option value="<+04>-4;85">(UTC+04:00) Tbilisi</option>
          <option value="<+04>-4;86">(UTC+04:00) Yerevan</option>
          <option value="<+0430>-4:30;87">(UTC+04:30) Kabul</option>
          <option value="<+05>-5;88">(UTC+05:00) Ashgabat, Tashkent</option>
          <option value="<+05>-5;89">(UTC+05:00) Ekaterinburg</option>
          <option value="PKT-5;90">(UTC+05:00) Islamabad, Karachi</option>
          <option value="<+05>-5;91">(UTC+05:00) Qyzylorda</option>
          <option value="IST-5:30;92">
            (UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi
          </option>
          <option value="<+0530>-5:30;93">
            (UTC+05:30) Sri Jayawardenepura
          </option>
          <option value="<+0545>-5:45;94">(UTC+05:45) Kathmandu</option>
          <option value="<+06>-6;95">(UTC+06:00) Astana</option>
          <option value="<+06>-6;96">(UTC+06:00) Dhaka</option>
          <option value="<+06>-6;97">(UTC+06:00) Omsk</option>
          <option value="<+0630>-6:30;98">(UTC+06:30) Yangon (Rangoon)</option>
          <option value="<+07>-7;99">
            (UTC+07:00) Bangkok, Hanoi, Jakarta
          </option>
          <option value="<+07>-7;100">
            (UTC+07:00) Barnaul, Gorno-Altaysk
          </option>
          <option value="<+07>-7;101">(UTC+07:00) Hovd</option>
          <option value="<+07>-7;102">(UTC+07:00) Krasnoyarsk</option>
          <option value="<+07>-7;103">(UTC+07:00) Novosibirsk</option>
          <option value="<+07>-7;104">(UTC+07:00) Tomsk</option>
          <option value="CST-8;105">
            (UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi
          </option>
          <option value="<+08>-8;106">(UTC+08:00) Irkutsk</option>
          <option value="<+08>-8;107">
            (UTC+08:00) Kuala Lumpur, Singapore
          </option>
          <option value="AWST-8;108">(UTC+08:00) Perth</option>
          <option value="CST-8;109">(UTC+08:00) Taipei</option>
          <option value="<+08>-8;110">(UTC+08:00) Ulaanbaatar</option>
          <option value="<+0845>-8:45;111">(UTC+08:45) Eucla</option>
          <option value="<+09>-9;112">(UTC+09:00) Chita</option>
          <option value="JST-9;113">(UTC+09:00) Osaka, Sapporo, Tokyo</option>
          <option value="KST-9;114">(UTC+09:00) Pyongyang</option>
          <option value="KST-9;115">(UTC+09:00) Seoul</option>
          <option value="<+09>-9;116">(UTC+09:00) Yakutsk</option>
          <option value="ACST-9:30ACDT,M10.1.0,M4.1.0/3;117">
            (UTC+09:30) Adelaide
          </option>
          <option value="ACST-9:30;118">(UTC+09:30) Darwin</option>
          <option value="AEST-10;119">(UTC+10:00) Brisbane</option>
          <option value="AEST-10AEDT,M10.1.0,M4.1.0/3;120">
            (UTC+10:00) Canberra, Melbourne, Sydney
          </option>
          <option value="<+10>-10;121">(UTC+10:00) Guam, Port Moresby</option>
          <option value="AEST-10AEDT,M10.1.0,M4.1.0/3;122">
            (UTC+10:00) Hobart
          </option>
          <option value="<+10>-10;123">(UTC+10:00) Vladivostok</option>
          <option value="<+1030>-10:30<+11>-11,M10.1.0,M4.1.0;124">
            (UTC+10:30) Lord Howe Island
          </option>
          <option value="<+11>-11;125">(UTC+11:00) Bougainville Island</option>
          <option value="<+11>-11;126">(UTC+11:00) Chokurdakh</option>
          <option value="<+11>-11;127">(UTC+11:00) Magadan</option>
          <option value="<+11>-11<+12>,M10.1.0,M4.1.0/3;128">
            (UTC+11:00) Norfolk Island
          </option>
          <option value="<+11>-11;129">(UTC+11:00) Sakhalin</option>
          <option value="<+11>-11;130">
            (UTC+11:00) Solomon Is., New Caledonia
          </option>
          <option value="<+12>-12;131">
            (UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky
          </option>
          <option value="NZST-12NZDT,M9.5.0,M4.1.0/3;132">
            (UTC+12:00) Auckland, Wellington
          </option>
          <option value="<+12>-12;133">
            (UTC+12:00) Coordinated Universal Time+12
          </option>
          <option value="<+12>-12;134">(UTC+12:00) Fiji</option>
          <option value="<+12>-12;135">
            (UTC+12:00) Petropavlovsk-Kamchatsky - Old
          </option>
          <option value="<+1245>-12:45<+1345>,M9.5.0/2:45,M4.1.0/3:45;136">
            (UTC+12:45) Chatham Islands
          </option>
          <option value="<+13>-13;137">
            (UTC+13:00) Coordinated Universal Time+13
          </option>
          <option value="<+13>-13;138">(UTC+13:00) Nuku'alofa</option>
          <option value="<+13>-13;139">(UTC+13:00) Samoa</option>
          <option value="<+14>-14;140">(UTC+14:00) Kiritimati Island</option>
        </select>
      </div>
    </div>
  );
};

export default CalendarSection;
