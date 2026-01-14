<Query Kind="Statements">
  <Connection>
    <ID>e0a87a77-277f-494c-93a7-51c2205344d2</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook-2025</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

//  year from 1990 to 1999
//  order Year
//  order Title
//Albums
//	.Where(a => a.ReleaseYear >= 1990 && a.ReleaseYear <= 1999)
//	.OrderBy(a => a.ReleaseYear)
//	.ThenBy(a => a.Title)
//	.Select(a => a)
//	.Dump();

//  year from 1990 to 1999
//  order Year
//  order by descending Title
Albums
.Where(a => a.ReleaseYear >= 1990 && a.ReleaseYear <= 1999)
.OrderBy(a => a.ReleaseYear)
.ThenByDescending(a => a.Title)
.Select(a => a)
.Dump();

Albums
	.Where(a => a.ReleaseYear >= 1990 && a.ReleaseYear <= 1999)
	.OrderBy(a => a.ReleaseYear)
	.ThenBy(a => a.ReleaseLabel)
	.ThenBy(a => a.Title)
	.Select(a => a)
	.Dump();




//  Showing Collections
//Albums.Dump("Albums")
//	.Where(a => a.ReleaseYear >= 1990 && a.ReleaseYear <= 1999)
//	.Dump("Filter/Unsorted")
//	.OrderBy(a => a.ReleaseYear)
//	.ThenBy(a => a.Title)
//	.Select(a => a)
//	.Dump("Filter/Sorted");