
export default function parseDelimitedValues(s: string): Record<string, string> {
  const headers = s.split(','); // Split by comma, ASSUME: commas in keys or values are encoded
  const o: Record<string, string> = {};

  headers.forEach((header) => {
    const [key, value] = header.split('='); // Split by equal sign
    if (key && value) {
      o[key.trim()] = value.trim(); // Add to the object, trimming spaces
    }
  });

  return o;
}
