// Copyright 2017, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201711;
using Google.Api.Ads.Dfp.v201711;
using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201711 {
  /// <summary>
  /// This example gets all sponsorship product templates.
  /// </summary>
  public class GetSponsorshipProductTemplates : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all sponsorship product templates.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetSponsorshipProductTemplates codeExample = new GetSponsorshipProductTemplates();
      Console.WriteLine(codeExample.Description);
      try {
        codeExample.Run(new DfpUser());
      } catch (Exception e) {
        Console.WriteLine("Failed to get product templates. Exception says \"{0}\"",
            e.Message);
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser dfpUser) {
      using (ProductTemplateService productTemplateService =
          (ProductTemplateService) dfpUser.GetService(DfpService.v201711.ProductTemplateService)) {

        // Create a statement to select product templates.
        int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
        StatementBuilder statementBuilder = new StatementBuilder()
            .Where("lineItemType = :lineItemType")
            .OrderBy("id ASC")
            .Limit(pageSize)
            .AddValue("lineItemType", LineItemType.SPONSORSHIP.ToString());

        // Retrieve a small amount of product templates at a time, paging through until all
        // product templates have been retrieved.
        int totalResultSetSize = 0;
        do {
          ProductTemplatePage page = productTemplateService.getProductTemplatesByStatement(
              statementBuilder.ToStatement());

          // Print out some information for each product template.
          if (page.results != null) {
            totalResultSetSize = page.totalResultSetSize;
            int i = page.startIndex;
            foreach (ProductTemplate productTemplate in page.results) {
              Console.WriteLine(
                  "{0}) Product template with ID {1} and name \"{2}\" was found.",
                  i++,
                  productTemplate.id,
                  productTemplate.name
              );
            }
          }

          statementBuilder.IncreaseOffsetBy(pageSize);
        } while (statementBuilder.GetOffset() < totalResultSetSize);

        Console.WriteLine("Number of results found: {0}", totalResultSetSize);
      }
    }
  }
}
