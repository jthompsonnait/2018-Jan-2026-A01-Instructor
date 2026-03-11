using BYSResults;
using HogWildSystem.DAL;
using HogWildSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem.BLL
{
    public class PartService
    {
        #region  Fields
        //  hogwild context
        private readonly HogWildContext _hogWildContext;
        #endregion

        //  constructor for the PartService class
        internal PartService(HogWildContext hogWildContext)
        {
            //  initialize the _hogWildContext field with the
            //       provided HogWildContext instance
            _hogWildContext = hogWildContext;
        }

        //	Get parts
        public Result<List<PartView>> GetParts(int? partCategoryID, string description, List<int> existingPartIDs)
        {
            //	Create a Result container that will hold either a 
            //		list of PartView on success or any accumulated errors on failure
            var result = new Result<List<PartView>>();

            #region Business Rules
            //	These are processing rules that need to be satisfied
            //		for valid data
            //		rule:	Both part id must be valid and/or description cannot be empty
            //		rule: 	Part IDs in existing part IDs will be ignored
            //		rule:	RemoveFromViewFlag must be false


            //	Both part id must be valid and/or description cannot be empty
            if (partCategoryID == 0 && string.IsNullOrWhiteSpace(description))
            {
                return result.AddError(new Error("Missing Information",
                                    "Please provide either a category and/or description"));
            }
            #endregion

            // Start with the base query, filtering out:
            // - any parts already in the "existing part IDs" list
            // - any parts flagged for removal from view
            var query = _hogWildContext.Parts
                .Where(p => !existingPartIDs.Contains(p.PartID) && !p.RemoveFromViewFlag);

            // Determine which search criteria the user actually provided.
            // A description is considered "provided" only if it's not null, empty, or whitespace.
            // A category is considered "provided" only if the ID is greater than zero.
            bool hasDescription = !string.IsNullOrWhiteSpace(description);
            bool hasCategory = partCategoryID > 0;

            if (hasDescription && hasCategory)
            {
                // Both search criteria were provided:
                // Filter parts where the description contains the search text (case-insensitive)
                // AND the part belongs to the specified category.
                // Using AND because we want to narrow results when both filters are active.
                query = query.Where(p =>
                    p.Description.ToUpper().Contains(description.ToUpper())
                    && p.PartCategoryID == partCategoryID);
            }
            else if (hasDescription)
            {
                // Only a description was provided (no category selected):
                // Filter parts where the description contains the search text (case-insensitive).
                query = query.Where(p =>
                    p.Description.ToUpper().Contains(description.ToUpper()));
            }
            else if (hasCategory)
            {
                // Only a category was provided (no description entered):
                // Filter parts that belong to the specified category.
                query = query.Where(p =>
                    p.PartCategoryID == partCategoryID);
            }
            // If neither criteria was provided, no additional filtering is applied,
            // and the base query (excluding existing parts and removed parts) is used as-is.

            // Project the filtered results into PartView objects,
            // selecting only the fields needed for the view.
            // Then sort alphabetically by description.
            // Finally, execute the query and materialize the results into a list.
            var parts = query
                .Select(p => new PartView
                {
                    PartID = p.PartID,
                    PartCategoryID = p.PartCategoryID,
                    CategoryName = p.PartCategory.Name,  // Navigation property to get the category name
                    Description = p.Description,
                    Cost = p.Cost,
                    Price = p.Price,
                    ROL = p.ROL,                         // Reorder Level
                    QOH = p.QOH,                         // Quantity On Hand
                    Taxable = p.Taxable,
                    RemoveFromViewFlag = p.RemoveFromViewFlag
                })
                .OrderBy(p => p.Description)    // Sort results alphabetically by description
                .ToList();                       // Execute the query against the database

            //  if no parts were found
            if (parts == null || parts.Count() == 0)
            {
                //need to exit because we did not find any parts
                return result.AddError(new Error("No Parts", "No parts were found"));
            }

            //	return the result
            return result.WithValue(parts);
        }

        //	Get the part
        public Result<PartView> GetPart(int partID)
        {
            // Create a Result container that will hold either a
            //	PartView objects on success or any accumulated errors on failure
            var result = new Result<PartView>();
            #region Business Rules
            //	These are processing rules that need to be satisfied
            //		rule:	partID must be valid
            //		rule: 	RemoveFromViewFlag must be false
            if (partID == 0)
            {
                //  need to exit because we have no part information
                return result.AddError(new Error("Missing Information",
                                "Please provide a valid part id"));
            }
            #endregion

            var part = _hogWildContext.Parts
                            .Where(p => (p.PartID == partID
                                         && !p.RemoveFromViewFlag))
                            .Select(p => new PartView
                            {
                                PartID = p.PartID,
                                PartCategoryID = p.PartCategoryID,
                                //  PartCategory is an alias for Lookup
                                CategoryName = p.PartCategory.Name,
                                Description = p.Description,
                                Cost = p.Cost,
                                Price = p.Price,
                                ROL = p.ROL,
                                QOH = p.QOH,
                                Taxable = (bool)p.Taxable,
                                RemoveFromViewFlag = p.RemoveFromViewFlag
                            }).FirstOrDefault();

            //  if no part were found
            if (part == null)
            {
                //  need to exit because we did not find any part
                return result.AddError(new Error("No part", "No part were found"));
            }

            //  return the result
            return result.WithValue(part);
        }
    }
}
