using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Runtime.InteropServices;
using Portia.Infrastructure.Core.Validators;
using System.CodeDom.Compiler;
using Portia.Infrastructure.Core.Portia.Main;
using Grasshopper.Kernel.Geometry.Delaunay;
using System.Text.RegularExpressions;

namespace Portia.Lite
{
    public static class Docs
    {
        public static string AddDropDownMark(
            this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return "< " + text;
        }

        public static string AddDropDownNote(
            this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return
                $"{text}{Environment.NewLine}Right-click to change the input structure!";
        }

        public static string DoubleRelation =>
            "Numerical comparison logic (Equal, GreaterThan, ..) for filtering floating point numbers.";

        public static string StringRelation =>
            "Text-based filter (Contains, StartsWith, ..) used mostly for node / edge Type processes.";

        public static string Gate =>
            "Boolean operator (AND, OR) to combine multiple conditions into one filter.";

        public static string DoubleValue =>
            "The target numerical value for mathematical filtering.";

        public static string StringValue =>
            "The target string value used for text-based filtering.";

        public static string PortiaComponent =>
            "The core building block of the Portia ecosystem, " +
            "designed to manage complex graph logic with architectural data " +
            "through customizable Task inputs.";

        public static string Condition =>
            "Defines a logical filter rule based on the relationship " +
            "with a selection relation type and a specific target value.";

        public static string DoubleCondition =>
            "Defines a logical filter rule based on the relationship " +
            "with a double-based selection relation type and a specific double target value.";

        public static string StringCondition =>
            "Defines a logical filter rule based on the relationship " +
            "with a text-based relation type and a specific string target value.";

        public static string GraphIdentity =>
            "Defines the primary identification rule for graph elements " +
            "using either a numerical Index or a text-based Type.";

        public static string GraphIdentityByIndex =>
            "Defines the primary identification rule for graph elements " +
            "using a numerical Index value.";

        public static string GraphIdentityByTag =>
            "Defines the primary identification rule for graph elements " +
            "using a text-based Type value.";

        public static string Index =>
            "The specific numerical index used to identify and map " +
            "a unique element (node or edge) within the graph structure.";

        public static string Type =>
            "A user-defined text string used to categorize and map " +
            "specific elements (nodes or edges) within the graph structure.";

        public static string Logic =>
            "Defines a localized graph process that evaluates " +
            "nodes or edges based on their geometric and topological behavior.";

        public static string NodeLogic =>
            "Defines a localized graph process that evaluates " +
            "nodes based on their geometric and topological behavior.";

        public static string EdgeLogic =>
            "Defines a localized graph process that evaluates " +
            "edges based on their geometric and topological behavior.";

        public static string NodeAdjacency =>
            "A rule that evaluates a Node's direct neighbouring Edge count.";

        public static string NodeProximity =>
            "A rule that checks the spatial distance between Nodes " +
            "regardless of direct edge connections.";

        public static string NodeVectorSum =>
            "A rule that calculates the resultant force or orientation of a node " +
            "by summing the directional vectors of all connected edges.";

        public static string IsLeafNode =>
            "A binary validation rule that identifies 'leaf' nodes—elements " +
            "with only one connection—at the termination of a graph branch.";

        public static string JointConstellation =>
            "A complex topological rule that identifies specific junction patterns and " +
            "branching arrangements within the wider network.";

        public static string EdgeLength =>
            "A rule that evaluates the physical length of an Edge curve.";

        public static string SourceAdjacency =>
            "A rule that evaluates the direct neighbouring Edge count of an Edge's start (Source) Node.";

        public static string TargetAdjacency =>
            "A rule that evaluates the direct neighbouring Edge count of an Edge's end (Target) Node.";

        public static string IsBridgeEdge =>
            "A topological rule that identifies critical 'bridge' edges whose removal " +
            "would split the graph into separate, disconnected parts.";

        public static string LinkConstellation =>
            "A complex rule that identifies Edges with similar EdgeType-to-EdgeType pattern.";

        public static string Task =>
            "A specialized graph operation that executes " +
            "data modifications, queries or validation protocols on the Portia engine.";

        public static string SetCurves =>
            "A primary Task that translates geometric curves into graph-compatible " +
            "edge data for network processing.";

        public static string SetNodeTypes =>
            "A modification Task that assigns specific Type string values to " +
            "existing graph Nodes for categorization.";

        public static string SetEdgeTypes =>
            "A modification Task that assigns specific Type string values to " +
            "existing graph Edges for categorization.";

        public static string GetNodes =>
            "A retrieval (query) Task that extracts Node identity values and geometry data " +
            "from the graph for downstream modeling.";

        public static string GetEdges =>
            "A retrieval (query) Task that extracts Edge identity values and geometry data " +
            "from the graph for downstream modeling.";

        public static string VerifyNodes =>
            "A validation Task that checks Nodes against selected logic rules " +
            "to ensure structural or topological integrity.";

        public static string VerifyEdges =>
            "A validation Task that checks Edges against selected logic rules " +
            "to ensure structural or topological integrity.";

        public static string Curves =>
            "The input geometric curves used to generate the " +
            "base network topology for the Portia engine.";

        public static string InitialEdgeTypes =>
            "The optional Type strings assigned to the Edges during the " +
            "initial graph generation process, used for categorization.";

        public static string Selection =>
            "A criteria used to isolate specific Nodes or Edges " +
            "for targeted graph modification or retrieval.";

        public static string Selections =>
            "A set of Selection criteria used to isolate specific Nodes or Edges " +
            "for targeted graph modification or retrieval.";

        public static string Types =>
            "The specific text-based Type values to be assigned to the " +
            "elements that get captured by the current input Selection logics.";

        public static string Logics =>
            "The graph logic rules used to evaluate and verify the topological or " +
            "geometric integrity of the elements that get captured " +
            "by the current input Selection logics.";

        public static string Name =>
            "A unique identifier used to label logic results and " +
            "dynamically name the corresponding Portia output fields for easy tracking.";

        public static string NodeVector =>
            "Defines a required Edge direction and " +
            "type mapping that must be present at a Node for a successful constellation match.";

        public static string StrictMatch =>
            "When enabled, the node must have an exact count of connected edges " +
            "matching the number of defined NodeVectors to pass the rule.";

        public static string AllowedSourceTypes =>
            "A list of valid identity Type strings for the start Node of an Edge " +
            "to satisfy the connection rule.";

        public static string AllowedTargetTypes =>
            "A list of valid identity Type strings for the end Node of an Edge " +
            "to satisfy the connection rule.";

        public static string Bidirectional =>
            "When enabled, the rule validates the connection regardless of edge direction, " +
            "checking both start-to-end and end-to-start orientations.";

        public static string GraphIdentitySelection =>
            "A selection rule that captures elements by matching their unique identity data, " +
            "such as specific Index numbers or text-based Types.";

        public static string LogicSelection =>
            "A selection rule that identifies elements based on whether they satisfy " +
            "a specific topological or geometric Logic condition.";

        public static string WrapSelection =>
            "A spatial selection rule that captures Nodes whose centroids are " +
            "contained within the volume of the specified input Breps.";

        public static string IntersectionSelection =>
            "A spatial selection rule that identifies Edges based on their physical intersection " +
            "AND containment with(in) the volumes of specified Breps.";

        public static string CompositeSelection =>
            "A sophisticated selection rule that combines multiple criteria " +
            "using a boolean Gate logic to capture graph elements.";

        public static string Wrappers =>
            "A list of boundary Breps used to define the spatial volumes " +
            "for Node centroid containment checks.";

        public static string Breps =>
            "The geometric boundary volumes used to evaluate intersections with " +
            "AND containment by Edges.";

        public static string StrictlyIn =>
            "A boolean toggle that determines if elements lying exactly on the " +
            "Brep boundary surface are included in the selection or not.";
    }
}