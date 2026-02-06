using System;

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

        public static string TemporaryUnused => "TODO";

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

        public static string Identity =>
            "Defines the primary identification rule for graph elements " +
            "using either a numerical Index or a text-based Type.";

        public static string IdentityByIndex =>
            "Defines the primary identification rule for graph elements " +
            "using a numerical Index value.";

        public static string IdentityByTag =>
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

        public static string IndexLogic =>
            "Defines a localized graph process that evaluates both " +
            "Nodes AND Edges based on their numerical Index.";

        public static string TypeLogic =>
            "Defines a localized graph process that evaluates both " +
            "Nodes AND Edges based on their text-based Type value.";

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

        public static string EdgeSourceIndex =>
            "A rule that evaluates / captures an Edge based on the numerical Index " +
            "of its starting (Source) Node.";

        public static string EdgeSourceType =>
            "A bridge rule that evaluates / captures an Edge based on the text-based Type " +
            "of its starting (Source) Node.";

        public static string EdgeTargetIndex =>
            "A bridge rule that evaluates / captures an Edge based on the numerical Index " +
            "of its ending (Target) Node.";

        public static string EdgeTargetType =>
            "A bridge rule that evaluates /captures an Edge based on the text-based Type " +
            "of its ending (Target) Node.";

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

        public static string ByLogicSelection =>
            "A selection rule that identifies elements based on whether they satisfy " +
            "a specific topological or geometric Logic condition.";

        public static string ByWrapSelection =>
            "A spatial selection rule that captures Nodes whose centroids are " +
            "contained within the volume of the specified input Breps.";

        public static string ByIntersectionSelection =>
            "A spatial selection rule that identifies Edges based on their physical intersection " +
            "AND containment with(in) the volumes of specified Breps.";

        public static string ByCompositeSelection =>
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

        public static string DeconstructItem =>
            "A helper component used to unlock the details of a graph element (both Node or Edge), " +
            "extracting its identity, geometric content (Centroid or Curve), and its raw JSON format data. " +
            "Connect to Portia Nodes and Edges outputs!";

        public static string GraphItem =>
            "The proprietary 'Sovereign Goo' wrapper that encapsulates a graph element (Node or Edge), " +
            "carrying its full topological intelligence and metadata through the network.";

        public static string Json =>
            "The universal, transaction-ready string representation of a graph element (Node or Edge), " +
            "allowing for seamless data exchange between Portia and external logic engines.";

        public static string Geometries =>
            "The projected geometric representation of an element: a Node's Centroid or Edge's Curve.";
    }
}